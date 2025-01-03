import './App.css';
import { Typography } from '@mui/material';
import {Routes, Route, Navigate, useNavigate, useLocation} from 'react-router-dom'
import BreedingGuide from './Components/BreedingGuide';
import Bestiary from './Components/Bestiary';
import TravelersGates from './Components/TravelersGates';
import { useEffect, useRef } from 'react';

const App = () => {

  const ROUTE_BESTIARY = '/Bestiary'
  const ROUTE_TRAVELERS_GATES = '/TravelersGates';
  const ROUTE_BREEDING_GUIDE = '/BreedingGuide';

  const bestiaryNavButton = useRef(null);
  const travelersGateNavButton = useRef(null);
  const breedingGuideNavButton = useRef(null);

  const navigate = useNavigate();
  const location = useLocation();

  const styleNavigationButton = (target, path) =>
  {
    if(path === location.pathname) {
      target.current.style.borderBottom = '5px solid #011B26';
    }
    else
    {
      target.current.style.borderBottom = 'none';
    }
  }

  const onNavigationButtonClicked = (path) => {
    navigate(path);
  }

  useEffect(() => {
      //When the component mounts, determine which (active) button
      //to underline based on the current route rendered.
      styleNavigationButton(bestiaryNavButton, ROUTE_BESTIARY);
      styleNavigationButton(travelersGateNavButton, ROUTE_TRAVELERS_GATES);
      styleNavigationButton(breedingGuideNavButton, ROUTE_BREEDING_GUIDE);
    }
  );

  return (
    <div className='App'>
      <header className='App-header'>
        <div style={{display: 'flex', flexDirection: 'column'}}>
          <img src='/TitleLogo.png' className='App-logo' alt='Dragom Warrior Monsters'/>
          <Typography variant='button' color='#FF5900' fontWeight={700}>COMPANION</Typography>
        </div>
      </header>
      <div className='Menu-header'>
        <button 
          className='Menu-button' 
          ref={bestiaryNavButton}
          onClick={() => onNavigationButtonClicked(ROUTE_BESTIARY)}>
            BESTIARY
        </button>
        <button 
          className='Menu-button' 
          ref={travelersGateNavButton}
          onClick={() => onNavigationButtonClicked(ROUTE_TRAVELERS_GATES)}>
            TRAVELER'S GATES
        </button>
        <button 
          className='Menu-button' 
          ref={breedingGuideNavButton}
          onClick={() => onNavigationButtonClicked(ROUTE_BREEDING_GUIDE)}>
            BREEDING GUIDE
        </button>
      </div>
      <Routes>
        <Route path='/' element={<Navigate to={ROUTE_BESTIARY} />}/>
        <Route path={ROUTE_BESTIARY} element={<Bestiary />} />
        <Route path={ROUTE_TRAVELERS_GATES} element={<TravelersGates />} />
        <Route path={ROUTE_BREEDING_GUIDE} element={<BreedingGuide />} />
      </Routes>
    </div>
  );
}

export default App;
