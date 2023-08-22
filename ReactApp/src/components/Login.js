import { useEffect, useState } from "react";
import { Link, useNavigate  } from "react-router-dom";
import { LoginUser, LoginGoogle, GetUserAfterLogin } from "../services/UserService";
import { GetRole, SetEmail, SetRole, SetToken, SetUser, userLoginModel, userModel } from "../models/UserModel";
import jwt from 'jwt-decode';


const Login = () => {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState({});
    const history = useNavigate();

    const handleSubmit = async(e) => {
        e.preventDefault();

        const validationErrors = {};

        if (!validateEmail(email)) {
          validationErrors.email = "Invalid email address.";
        }
        if (password.length < 3 || password.length > 20) {
          validationErrors.password = "Password must be between 3 and 20 characters.";
        }

        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }

        const account = userLoginModel;
        account.email = email;
        account.password = password;    

        try{
            const response = await LoginUser(account);
            SetToken(response.data);
            SetRole(jwt(response.data));
            SetEmail(account.email);

            if(GetRole() === "prodavac")
                //await CheckVerification(response.data);
                
            history("/");     
            window.location.reload()
        }
        catch(e){
            if(e.response.status === 401 || e.response.status === 403)
            {
              localStorage.clear();
              history('/');
            }
            alert('Ne postoji account sa tim email i lozinkom! Pokusajte ponovo.');
            history("/login");
        }
    }

    
    // const CheckVerification = async(token) => {
    //     try{
    //         const responseUser = await GetUserAfterLogin(token);
    //         const response = await GetVerificationFromBackend(responseUser.data.id, token);
    //         if(response.data.status === 0)
    //         {
    //           SetVerification('In process');
    //         }
    //         else if(response.data.status === 1)
    //         {
    //           SetVerification('Accepted');
    //         }
    //         else
    //         {
    //           SetVerification('Denied');
    //         }
    //     }
    //     catch(e){
    //         if(e.response.status === 401 || e.response.status === 403)
    //         {
    //           localStorage.clear();
    //           history('/');
    //         }
    //     }
        
    // }



    const validateEmail = (email) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    return ( 
        <div className="login">
            <h2>Prijavite se</h2>
            <form onSubmit={handleSubmit}>
                <label>Email: </label>
                <input 
                    type="text"
                    required   
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                {errors.email && <p className="error">{errors.email}</p>}
                <label>Password: </label>
                <input
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                {errors.password && <p className="error">{errors.password}</p>}
                <div id="signInDiv"></div>
                <br/>
                <Link to='/registracija'>Ako nemate profil, pritisnite ovaj link za registraciju</Link>
                <button className="button-login">Log in</button>
            </form>
        </div>
     );
}
 
export default Login;