import './BreedingGuide.css'
import { useEffect, useState } from "react";
import axios from "axios";
import { Autocomplete, TextField, Input, Button } from "@mui/material";
import { useRef } from 'react';
import BreedingChecklistPopup from './BreedingChecklistPopup';

const BreedingGuide = 
    () => 
    {
   
        const [monsterList, setMonsterList] = useState([]);
        const [monsterImageCache, setMonsterImageCache] = useState();
        const [familyImageCache, setFamilyImageCache] = useState();
        const [targetMonsterName, setTargetMonsterName] = useState("");
        const [maxHierarchyLevels, setMaxHierarchyLevels] = useState("");
        const [familyTree, setFamilyTree] = useState();
        const [lastMousePos, setLastMousePos] = useState({x: 0, Y: 0});
        const [camera, setCamera] = useState({x: 0, y: 0, isPanning: false});
        const [pixelDensity, setPixelDensity] = useState(1);
        const canvas = useRef(null);
        
        const NODE_PADDING = 30;
        const NODE_WIDTH = 115;
        const NODE_HEIGHT = 140;
        const NODE_IMAGE_WIDTH = 75;
        const NODE_IMAGE_HEIGHT = 75;
        const SHORTCUT_IMAGE_WIDTH = NODE_IMAGE_WIDTH * 0.7;
        const SHORTCUT_IMAGE_HEIGHT = NODE_IMAGE_HEIGHT * 0.7;

        let clickableElements = [];

        const loadMonsterList = 
            () => 
            {
                axios
                    .get(`http://localhost:5000/api/MonsterDetails`)
                    .then(
                        response =>
                        {
                            //Load monster list and monster image cache
                            const monsterData = response.data;
                            setMonsterList(monsterData.map(monsterDetails => monsterDetails.monsterName).sort());
                            
                            const monsterImageMap = new Map();
                            const familyImageMap = new Map();

                            monsterData.forEach
                            (
                                (monster) =>
                                {
                                    //Cache an image for the monster
                                    const monsterImg = new Image();
                                    monsterImg.src = `/monster_images/${monster.monsterName}.png`
                                    monsterImageMap.set(monster.monsterName, monsterImg);

                                    //If an image for the monster's family has not yet been cached, cache one.
                                    if(!familyImageMap.has(monster.family))
                                    {
                                        const familyImg = new Image();
                                        familyImg.src = `/family_images/${monster.family}.png`
                                        familyImageMap.set(monster.family, familyImg);
                                    }
                                }
                            );

                            setMonsterImageCache(monsterImageMap);
                            setFamilyImageCache(familyImageMap)
                        }
                    )
                    .catch
                    (
                        error =>
                        {
                            console.error(`Error fetching monster data: ${error}`)
                        }
                    );
            };  

        const calculateVirtualCanvasWidth = 
            (maxHierarchyLevel) => (2 ** maxHierarchyLevel) * (NODE_WIDTH + NODE_PADDING);

        const calculateCanvasDisplayWidth =
            () => +getComputedStyle(canvas.current).getPropertyValue("width").slice(0, -2);

        const loadMonsterFamilyTreeData = 
            () => 
            {
                axios
                    .get(`http://localhost:5000/api/MonsterFamilyTree/${targetMonsterName}${(maxHierarchyLevels ? "?maxHierarchyLevels=" + maxHierarchyLevels : "")}`)
                    .then(
                        response => 
                        {
                            setFamilyTree(response.data);
                            const rootNodePosition = calculateVirtualCanvasWidth(response.data.maxHierarchyLevel) / 2;

                            //Adjust the camera position to center on the (anticipated position) of the new target node.
                            setCamera
                            (
                                {
                                    x: rootNodePosition - (calculateCanvasDisplayWidth() / 2),
                                    y: 0,
                                    isPanning: camera.isPanning
                                }
                            );

                            renderFamilyTree();
                        }
                    )
                    .catch
                    (
                        error => 
                        {
                            console.error(`Error fetching family tree data: ${error}`)
                        }
                    );
            };

        const renderFamilyTree = 
            () => 
            {
                if(!familyTree)
                {
                    return;
                }

                //Clear the clickable elements so they are re-created in the correct spot
                clickableElements = [];

                const ctx = canvas.current.getContext('2d');
                ctx.textAlign = "center";
                ctx.textBaseline = "top";
                ctx.font = "bold 12px sans-serif";

                //For the 'virtual' canvas width, we must reserve enough horizontal space to
                //draw a full family tree at n-generations (2 ^ n * (node width + padding))
                const V_WIDTH = calculateVirtualCanvasWidth(familyTree.maxHierarchyLevel);
                const V_HEIGHT = familyTree.maxHierarchyLevel * (NODE_HEIGHT + NODE_PADDING);
                
                //TODO (Debug Code): Remove this later.
                //Draw a debug rect around the virtual space allocated
                ctx.strokeStyle = "red";
                ctx.strokeRect
                (
                    -camera.x,
                    -camera.y,
                    V_WIDTH,
                    V_HEIGHT
                )
                ctx.strokeStyle = "black";

                const renderNode = 
                    (node, nodeImageType, parentNode) =>
                    {
                        const addClickableImage =
                            (img, x, y, w, h, onClickCallback) =>
                            {
                                ctx.drawImage(img, x, y, w, h);

                                clickableElements
                                    .push
                                    (
                                        {
                                            rect:
                                            {
                                                x: x,
                                                y: y,
                                                w: w,
                                                h: h
                                            },
                                            clickCallback: onClickCallback,
                                            intersects: 
                                                (clickCoords) =>
                                                    clickCoords.x >= x &&
                                                    clickCoords.x <= x + w &&
                                                    clickCoords.y >= y &&
                                                    clickCoords.y <= y + h
                                        }
                                    )
                            };

                        //The canvas must be divided into enough space to position all members of the current generation.
                        //A single 'span' is equal to 2^g where g is the current generational (node) level of the hierarchy.
                        const hierarchyLevelSpan = V_WIDTH / (2 ** node.nodeLevel);
                        const nextHierarchyLevelSpan = V_WIDTH / (2 ** (node.nodeLevel + 1));

                        //Position the node along the x-axis either one span to the left (for pedigree) or one span to the right (for partner)
                        const nodePosX = (parentNode?.nodePosX ?? 0) + (hierarchyLevelSpan * (node.nodeType === "Pedigree" ? -1 : 1));

                        //Position it along the y-axis based on its generational level (made zero-based) and padding
                        const nodePosY = ((node.nodeLevel - 1) * (NODE_HEIGHT + NODE_PADDING)) + (NODE_PADDING / 2);

                        const img = 
                            nodeImageType === "Monster" ? 
                                monsterImageCache.get(node.nodeName) :
                                familyImageCache.get(node.nodeName);
                        
                        const nodeScreenspaceX = nodePosX - camera.x;
                        const nodeScreenspaceY = nodePosY - camera.y;
                        const currentDisplayWidth = calculateCanvasDisplayWidth() * pixelDensity;

                        try
                        {
                            if(node.nodeLevel !== 1)
                            {
                                const pathX = nodeScreenspaceX + (NODE_WIDTH / 2);
                                const pathEndY = nodeScreenspaceY - (NODE_HEIGHT / 2) - NODE_PADDING;

                                //If the node is not at the topmost hierarchy level, draw a line to connect to the next level up
                                ctx.beginPath();
                                ctx.moveTo(pathX, nodeScreenspaceY);
                                ctx.lineTo(pathX, pathEndY);
                                ctx.stroke();

                                if
                                (
                                    pathX > 0 && pathX <= currentDisplayWidth &&
                                    (
                                        ((parentNode.nodePosX - camera.x) + NODE_WIDTH) <= 0 ||
                                        ((parentNode.nodePosX - camera.x) ) > currentDisplayWidth
                                    )
                                )
                                {
                                    addClickableImage
                                    (
                                        monsterImageCache.get(parentNode.nodeName),
                                        pathX - (SHORTCUT_IMAGE_WIDTH / 2),
                                        nodeScreenspaceY - (SHORTCUT_IMAGE_HEIGHT / 2) - ((nodeScreenspaceY - pathEndY) / 2),
                                        SHORTCUT_IMAGE_WIDTH,
                                        SHORTCUT_IMAGE_HEIGHT,
                                        () =>
                                            {
                                                setCamera
                                                (
                                                    {
                                                        x: parentNode.nodePosX - (currentDisplayWidth / 2),
                                                        y: parentNode.nodePosY - (NODE_IMAGE_HEIGHT * 2),
                                                        isPanning: false
                                                    }
                                                )
                                            }
                                    );
                                }
                            }

                            //Draw a rectangle around the node
                            ctx.strokeRect
                            (
                                nodeScreenspaceX, 
                                nodeScreenspaceY,
                                NODE_WIDTH,
                                NODE_HEIGHT
                            );

                            //Position the image within its node, and draw it (relative to the camera position)
                            const imagePadding = 
                                {
                                    x: (NODE_WIDTH - NODE_IMAGE_WIDTH) / 2,
                                    y: (NODE_HEIGHT - NODE_IMAGE_HEIGHT) / 4
                                };

                            //Label the node type
                            ctx.fillText
                            (
                                node.nodeType.toUpperCase(), 
                                nodeScreenspaceX + (NODE_WIDTH / 2), 
                                nodeScreenspaceY + (imagePadding.y / 4), 
                                NODE_WIDTH
                            );

                            //Draw the node image
                            ctx.drawImage
                            (
                                img, 
                                nodeScreenspaceX + imagePadding.x, 
                                nodeScreenspaceY + imagePadding.y, 
                                NODE_IMAGE_WIDTH, 
                                NODE_IMAGE_HEIGHT
                            );

                            //Add the monster name or family
                            ctx.fillText
                            (
                                node.nodeName,
                                nodeScreenspaceX + (NODE_WIDTH / 2), 
                                nodeScreenspaceY + (imagePadding.y * 1.5) + NODE_IMAGE_HEIGHT,
                                NODE_WIDTH
                            )

                            if(nodeImageType === "Family")
                            {
                                ctx.fillText
                                (
                                    '(' + nodeImageType.toUpperCase() + ')',
                                    nodeScreenspaceX + (NODE_WIDTH / 2), 
                                    nodeScreenspaceY + (imagePadding.y * 2.5) + NODE_IMAGE_HEIGHT,
                                    NODE_WIDTH
                                )
                            }
                        }
                        catch(ex)
                        {
                            console.error(`Error rendering node '${node.nodeName}' (${node.nodePath}): ${ex}`);
                        }

                        //Draw the lineage (and icon if applicable) between the offspring (parent node) and either its pedigree or partner node (the target node)
                        const drawLineage =
                            (lineageType, targetNode) =>
                            {
                                const lineStartX = 
                                    lineageType === "Pedigree" ? 
                                        nodeScreenspaceX : 
                                        nodeScreenspaceX + NODE_WIDTH;

                                const lineEndX = 
                                    lineageType === "Pedigree" ?
                                        nodeScreenspaceX - nextHierarchyLevelSpan + (NODE_WIDTH / 2) :
                                        nodeScreenspaceX + NODE_WIDTH + nextHierarchyLevelSpan - (NODE_WIDTH / 2);

                                const lineY = nodeScreenspaceY + (NODE_HEIGHT / 2);

                                ctx.beginPath();
                                ctx.moveTo(lineStartX, lineY);
                                ctx.lineTo(lineEndX, lineY);
                                ctx.stroke();

                                //Draw shortcut icons for the lineage if the line's starting point is far enough onto 
                                //the screen to show the icon and the node needing a shortcut is completely offscreen.
                                const showShortcutIcon = 
                                    (
                                        lineageType === "Pedigree" &&
                                        lineStartX > (NODE_IMAGE_WIDTH * pixelDensity) &&                         
                                        (targetNode.nodePosX - camera.x) < (-NODE_WIDTH)          
                                    ) ||
                                    (
                                        lineageType === "Partner" &&
                                        currentDisplayWidth - lineStartX > (NODE_IMAGE_WIDTH) &&   
                                        (targetNode.nodePosX - camera.x) > canvas.current.width  
                                    )

                                if(showShortcutIcon)
                                {
                                    const imgNodeType = lineageType === "Pedigree" ? node.pedigreeType : node.partnerType;
                                    const imgNodeName = lineageType === "Pedigree" ? node.pedigreeName : node.partnerName;

                                    const shortcutImg = 
                                        imgNodeType === "Monster" ? 
                                            monsterImageCache.get(imgNodeName) :
                                            familyImageCache.get(imgNodeName);
                                    
                                    //Calculate where to show the icon based on the amount of the line on screen
                                    const shortcutImagePositionX = 
                                        lineageType === "Pedigree" ?
                                            Math.min
                                            (
                                                nodePosX - camera.x,
                                                currentDisplayWidth
                                            ) / 2 :
                                            Math.max
                                            (
                                                lineStartX + ((currentDisplayWidth - lineStartX) / 2),
                                                currentDisplayWidth / 2
                                            );

                                    addClickableImage
                                    (
                                        shortcutImg, 
                                        shortcutImagePositionX - (SHORTCUT_IMAGE_WIDTH / 2), 
                                        lineY - (SHORTCUT_IMAGE_HEIGHT / 2), 
                                        SHORTCUT_IMAGE_WIDTH, 
                                        SHORTCUT_IMAGE_HEIGHT,
                                        () =>
                                        {
                                            setCamera
                                            (
                                                {
                                                    x: targetNode.nodePosX - (currentDisplayWidth / 2),
                                                    y: targetNode.nodePosY - (NODE_IMAGE_HEIGHT * 2),
                                                    isPanning: false
                                                }
                                            )
                                        }
                                    );
                                }
                            };

                        //Recursively draw each node's pedigree and partner (and lines from the current node to them)
                        if(node.pedigreeNode != null)
                        {
                            const pedigreeNode = renderNode(node.pedigreeNode, node.pedigreeType, {nodePosX, nodePosY, nodeName: node.nodeName});
                            drawLineage("Pedigree", pedigreeNode);
                        }

                        if(node.partnerNode != null)
                        {
                            const partnerNode = renderNode(node.partnerNode, node.partnerType, {nodePosX, nodePosY, nodeName: node.nodeName});
                            drawLineage("Partner", partnerNode);
                        }

                        return {nodePosX, nodePosY};
                    };

                if(canvas?.current != null && familyTree.rootNode)
                {
                    //Render the root node and recursively render each of its children
                    renderNode(familyTree.rootNode, "Monster", null);
                }
            };
        
        const handleSubmitButtonClicked = 
            () => 
            {
                if(targetMonsterName)
                {
                    loadMonsterFamilyTreeData();
                }
            };

        const adjustCanvasResolution =
            () =>
            {
                //get DPI
                setPixelDensity(window.devicePixelRatio);

                //get CSS height
                //the + prefix casts it to an integer; the slice method gets rid of "px"
                let style_height = +getComputedStyle(canvas.current).getPropertyValue("height").slice(0, -2);
                
                //get CSS width
                let style_width = calculateCanvasDisplayWidth();
                
                //scale the canvas
                canvas.current.setAttribute('height', style_height * pixelDensity);
                canvas.current.setAttribute('width', style_width * pixelDensity);
            };

        const handleCanvasMouseDown = 
            (evt) =>
            {
                setCamera
                (
                    {
                        x: camera.x,
                        y: camera.y,
                        isPanning: true
                    }
                );

                const {offsetX, offsetY} = evt.nativeEvent;
                setLastMousePos({x: offsetX, y: offsetY});
            };

        let handleCanvasMouseLeave;
        const handleCanvasMouseUp = handleCanvasMouseLeave =
            () =>
            {
                setCamera
                (
                    {
                        x: camera.x,
                        y: camera.y,
                        isPanning: false
                    }
                );
            }

        const handleCanvasMouseMove =
            (evt) =>
            {
                const {offsetX, offsetY} = evt.nativeEvent;
                //console.log(`Mouse Screen Space: {x:${offsetX}, y:${offsetY}}  |  Camera Pos: {x:${camera.x}, y:${camera.y}}  |  Mouse World Space: {x:${offsetX + camera.x}, y:${offsetY + camera.y}}`);

                if(camera.isPanning)
                {
                    const deltaX = lastMousePos.x - offsetX;
                    const deltaY = lastMousePos.y - offsetY;

                    //console.log(`Mouse Delta: ${deltaX}, ${deltaY}`);

                    setCamera
                    (
                        {
                            x: camera.x + deltaX,
                            y: camera.y + deltaY,
                            isPanning: true
                        }
                    );

                    setLastMousePos({x: offsetX, y: offsetY});
                }
            };

        const handleCanvasMouseClick = 
            (evt) =>
            {
                const {offsetX, offsetY} = evt.nativeEvent;

                clickableElements.forEach
                (
                    element =>
                    {
                        if(element.intersects({x: offsetX * pixelDensity, y: offsetY * pixelDensity}))
                        {
                            element.clickCallback();
                            return;
                        }
                    }    
                )
            };

        useEffect
        (    
            () =>
            {
                if(monsterList.length === 0)
                {
                    loadMonsterList();
                }

                const onWindowResized = 
                    () =>
                    {
                        adjustCanvasResolution();
                        if(familyTree)
                        {
                            renderFamilyTree();
                        }
                    };

                onWindowResized();
                window.addEventListener('resize', onWindowResized);
                return () => window.removeEventListener('resize', onWindowResized);
            }
        );

        useEffect
        (
            () =>
            {
                renderFamilyTree();
            },
            [familyTree]
        );

        return (
            <div className='BreedingGuideContainer'>
                {/*<BreedingChecklistPopup/>*/}
                <div className='OptionsContainer'>
                    <Autocomplete options={monsterList} sx={{width:300}}
                        renderInput={(params) => <TextField {...params} label="Target Monster" />}
                        onChange={(event, newVal) => setTargetMonsterName(newVal)} />
                    <Input type='number' sx={{padding:{left:30}, width:220}} 
                        label="Max Hierarchy Levels"
                        placeholder='Enter Max Levels to View'
                        variant='outlined'
                        value={maxHierarchyLevels}
                        onChange={(event) => setMaxHierarchyLevels(event.target.value)}/>
                    <Button variant='contained' onClick={handleSubmitButtonClicked} 
                        sx={{width: 200, height: 50, padding:{left:80}}}
                        disabled={(!targetMonsterName)}>
                        View Family Tree
                    </Button>
                </div>
                <canvas ref={canvas} className='FamilyTreeViewer' 
                    onMouseDown={handleCanvasMouseDown}
                    onMouseUp={handleCanvasMouseUp}
                    onMouseLeave={handleCanvasMouseLeave}
                    onMouseMove={handleCanvasMouseMove}
                    onClick={handleCanvasMouseClick}/>
            </div>
        );
    }

export default BreedingGuide;