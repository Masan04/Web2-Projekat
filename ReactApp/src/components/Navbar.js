import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { GetRole, GetToken } from "../models/UserModel";


const Navbar = () => {

    const [isLoggedIn, setIsLoggedIn] = useState(GetToken() !== null);
    const [role, setRole] = useState(GetRole());
    const history = useNavigate();

    useEffect(() => {
        setIsLoggedIn(GetToken() !== null);
        if(isLoggedIn)
          // getData();
        setRole(GetRole());

      }, [role, isLoggedIn]);
      
      

    return (  
        <nav className="navbar">
            <h1>Online kupovina</h1>
            <div className="links">
                {isLoggedIn && <Link to="/">Home</Link>}
                {isLoggedIn && <Link to="/profil">Profil</Link>}  
            </div>
        </nav>
    );
}
 
export default Navbar;