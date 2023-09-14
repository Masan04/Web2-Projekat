import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Register from './components/Register';
import Login from './components/Login'
import Profile from './components/Profile';
import AddItem from './components/AddItem';
import { AuthUser } from './services/UserService';
import ProfileChange from './components/ProfileChange';
import Items from './components/Items';
import ChangeItem from './components/ChangeItem';
import MakeOrder from './components/MakeOrder';
import NewOrderBuyer from './components/NewOrderBuyer';
import MyOrder from './components/MyOrder';
import NewOrderSeller from './components/NewOrderSeller';
import Request from './components/Request';
import NotFoundPage from './components/NotFoundPage';
import PastOrders from './components/PastOrders';
import AllOrders from './components/AllOrders';
import Verification from './components/Verification';

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
              <Route path="/izmenaProfila" element={user ? <ProfileChange /> : <Navigate to="/" />} />
              <Route path="/artikli" element={user ? <Items /> : <Navigate to="/" />} />
              <Route path="/izmenaArtikla" element={user ? <ChangeItem /> : <Navigate to="/" />} />
              <Route path="/porudzbinaKupac" element={user ? <NewOrderBuyer /> : <Navigate to="/" />} />
              <Route path="/napraviPorudzbinu/:items" element={user ? <MakeOrder /> : <Navigate to="/" />} />
              <Route path="/mojePorudzbine" element={user ? <MyOrder /> : <Navigate to="/" />} />
              <Route path="/novePorudzbineProdavac" element={user ? <NewOrderSeller /> : <Navigate to="/" />} />
              <Route path="/prethodnePorudzbine" element={(user === "buyer") ? <PastOrders /> : <Navigate to="/" />} />
              <Route path="/napraviPorudzbinu/[]" element={user ? <Request /> : <Navigate to="/" />} />
              <Route path="/svePorudzbine" element={(user === "admin") ? <AllOrders/> : <Navigate to="/" />} />
              <Route path="/verification" element={(user === "admin") ? <Verification /> : <Navigate to="/" />} />
              <Route path="*" element={<NotFoundPage />} />
            </Routes>
          </div>
      </div>
    </BrowserRouter>
     </div>
  );
}

export default App;
