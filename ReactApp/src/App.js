import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Register from './components/Register';
import Login from './components/Login'
import Profile from './components/Profile';
import AddItem from './components/AddItem';
import { AuthUser } from './services/UserService';
import { GetStatus } from './models/UserModel';

function App() {

    const user = AuthUser();
  return (
    <div>
    <BrowserRouter>
      <div className="App">
        <Navbar/>
<         div className="content">
            <Routes>
              <Route path="/" element={<Home />}/>
              <Route path="/registracija" element={!user ? <Register /> : <Navigate to="/" />} />
              <Route path="/login" element={!user ? <Login /> : <Navigate to="/" />} />
              <Route path="/profil" element={user ? <Profile /> : <Navigate to="/" />} /> 
              <Route path="/noviArtikal" element={ user? <AddItem/> : <Navigate to ="/"/>} />
            </Routes>
          </div>
      </div>
    </BrowserRouter>
     </div>
  );
}

export default App;
