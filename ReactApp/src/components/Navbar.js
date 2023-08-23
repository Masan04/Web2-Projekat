import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { GetRole, GetToken, GetStatus } from "../models/UserModel";




const Navbar = () => {

  const [isLoggedIn, setIsLoggedIn] = useState(GetToken() !== null);
  const [role, setRole] = useState(GetRole());
  const [status, setStatus] = useState(GetStatus());

  useEffect(() => {

    setIsLoggedIn(GetToken() !== null);
    if (isLoggedIn){
    setRole(GetRole());
    setStatus(GetStatus());
    }
    

  }, [role, isLoggedIn, status]);

  return (
    <nav className="navbar">
      <h1>Online kupovina</h1>
      <div className="links">
        {isLoggedIn && <Link to="/">Home</Link>}
        {isLoggedIn && <Link to="/profil">Profil</Link>}
        {isLoggedIn && role === "seller" && status && <Link to="/noviArtikal">Dodaj artikal</Link>}
      </div>
    </nav>
  );
}

export default Navbar;