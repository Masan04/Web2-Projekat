import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { SetUser, userModel } from "../models/UserModel";
import mr from '../mr.png'
import { AuthUser, GetUserFromBackend } from '../services/UserService';

const Profile = () => {
    const history = useNavigate();
    const [user, setUser] = useState(userModel);
    const [isSeller, setIsSeller] = useState(false);
    const [isVerified, SetIsVerified] = useState(false);
    const [verification, SetVerification] = useState('');
     
    useEffect(() => {   
        getData();
    
      }, [user.type, user.status])


      const getData = async () => {
        let user = userModel;
        try{
          const response = await GetUserFromBackend();
          user = response.data;
          SetUser(response.data);
          
          if (user.status === 0){
            SetVerification('U procesu')
          }else if (user.status === 1){
            SetVerification('Verifikovan')
            SetIsVerified(true);
          }else{
            SetVerification('Odbijen')
          }
      
        }
        catch(e){
          if(e.response.status === 401 || e.response.status === 403)
          {
            localStorage.clear();
            history('/');
          }
        }
  
        const temp = user;
        if (user.type === 1){
          temp.type = 'Prodavac';
          setIsSeller(true);
        }
        else if (user.type === 2) {
          temp.type = 'Kupac'
          setIsSeller(false);
        }
        else{
          temp.type = 'Admin';
          setIsSeller(false);
        }
  
        temp.password = (user.password).slice(0, 10).split('').map(() => '*').join('');
        setUser(temp);
      }

    return ( 
        <div className="profile-container">
        <h2>Profil: </h2>
        <div className="profile-picture">
          <img src={mr} alt="Profilna slika:" />
        </div>
        <div className="user-info">
          <div>
            <strong>Korisnicko ime:</strong> {user.username}
          </div>
          <div>
            <strong>Lozinka:</strong> {user.password}
          </div>
          <div>
            <strong>Email:</strong> {user.userEmail}
          </div>
          <div>
            <strong>Ime:</strong> {user.name}
          </div>
          <div>
            <strong>Prezime:</strong> {user.surname}
          </div>
          <div>
            <strong>Datum rodjenja:</strong> {user.date}
          </div>
          <div>
            <strong>Adresa:</strong> {user.address}
          </div>
          <div>
            <strong>Tip korisnika:</strong> {user.type}
          </div>
          {(!isSeller || isVerified) && <Link className="buttonProfile" to="/izmenaProfila">Izmena profila</Link>}
        </div>
        { isSeller &&
        <>
        <div className="verification-box">
          <h2>Verification status</h2>
          <p>{verification}</p>
        </div>
        </>}
      </div>
     );
}
 
export default Profile;
