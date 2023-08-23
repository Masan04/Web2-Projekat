import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from 'react';
import { GetToken, userModel, SetUser, SetStatus, GetEmail } from "../models/UserModel";
import { GetUserFromBackend } from '../services/UserService';


const Home = () => {

  const history = useNavigate();
  const isLoggedIn = GetToken() !== null;
  const [user, setUser] = useState(userModel);
  const [isVerified, setIsVerified] = useState(false);
  const [status, SetUserStatus] = useState(false);


  useEffect(() => {
    if (GetEmail() != undefined) {
      getData();
    }

  }, [user.status, isVerified, status])

  const getData = async () => {
    let user = userModel;
    try {
      const response = await GetUserFromBackend();
      user = response.data;
      SetUser(response.data)    
    }
    catch (e) {
      if (e.response.status === 401 || e.response.status === 403) {
        localStorage.clear();
        history('/');
      }
    }

  }
  const handleLogout = () => {
    localStorage.clear();
    history("/");
    window.location.reload();
  };

  return (
    <div className="home">
      <h2>Dobrodosli na pocetnu stranicu</h2>
      {!isLoggedIn && (
        <>
          <p>Za pristup kupovini i prodaji, molimo Vas da se ulogujete.</p>
          <Link to="/registracija">Ukoliko nemate profil, pritisnite ovaj link za registraciju.</Link>
          <Link className="button" to="/login">Prijavi se</Link>
        </>
      )}
      {isLoggedIn && (
        <>
          <p>Pritiskom na dugme ispod mozete se izlogovati sa nase stranice.</p>
          <button className="button" onClick={() => handleLogout()}>Odjavi se</button>
        </>
      )}
    </div>
  );
}

export default Home;