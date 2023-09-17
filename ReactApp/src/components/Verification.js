import React, { useState, useEffect } from 'react';
import { GetAllUsers, SetVerification } from '../services/UserService';
import emailjs from '@emailjs/browser';
import { useNavigate } from 'react-router-dom';

const Verification = () => {

  const [verifications, setVerifications] = useState([]);
  const [sellers, setSellers] = useState([]);
  const history = useNavigate();

  useEffect(() => {
    getSellers();
  }, []);

  const getSellers = async () => {
    const response = await GetAllUsers();
    const temp = response.data.map(user => ({
      ...user,
      password: user.password.slice(0, 10).split('').map(() => '*').join('')
    }));
    setSellers(temp);
  };

  const handleAccept = async (id, userEmail) => {
    const response = await SetVerification(id, 1);
    console.log(response);
    sendEmail("Vas nalog je prihvacen. Sada mozete koristiti funkcionalnosti nase stranice.", userEmail)
  };

  const handleDeny = async (id, userEmail) => {
    const response = await SetVerification(id, 2);
    console.log(response);
    sendEmail("Vas nalog je odbijen i nazalost ne mozete koristiti funkcionalnosti nase stranice.", userEmail)
  };


  const sendEmail = (message, sellerEmail) => {
    const email = { message: message, emailTo: sellerEmail }
    emailjs.send(process.env.REACT_APP_SERVICE_ID, process.env.REACT_APP_SERVICE_TEMPLATE, email, process.env.REACT_APP_SERVICE_PUBLIC_KEY)
      .then(function (response) {
        console.log('SUCCESS!', response.status, response.text);
        window.location.reload();
      }, function (error) {
        console.log('FAILED...', error);
      });


  }

  return (
    <div className="verification-container">
      <div className="verification-content">
        <h2 className="verification-title">Korisnici</h2>
        <table className="verification-table">
          <thead>
            <tr>
              <th>UserId</th>
              <th>Korisnicko ime</th>
              <th>Email</th>
              <th>Password</th>
              <th>Ime</th>
              <th>Prezime</th>
              <th>Datum rodjenja</th>
              <th>Adresa</th>
              <th>Slika</th>
              <th>Tip</th>
              <th>Status</th>
              <th>Accept</th>
              <th>Deny</th>
            </tr>
          </thead>
          <tbody>
            {sellers.map((seller) => {
              return (
                <tr key={seller.id}>
                  <td>{seller.id}</td>
                  <td>{seller.username}</td>
                  <td>{seller.userEmail}</td>
                  <td>{seller.password}</td>
                  <td>{seller.name}</td>
                  <td>{seller.surname}</td>
                  <td>{seller.date}</td>
                  <td>{seller.address}</td>
                  <td>{seller.picture}</td>
                  <td>{(seller.type === 1 && <label>prodavac</label>) || (seller.type === 2 && <label>kupac</label>)}</td>
                  <td>{(seller.status === 0 && seller.type !== 2 && <label>U procesu</label>) || (seller.status === 1 && <label>Prihvaćen</label>) || (seller.status === 2 && <label>Odbijen</label>) || ((seller.type === 2 || seller.type === 0) && <label>-</label>)}</td>
                  {seller.type === 1 ? (
                    <>
                      <td>
                        <button
                          className="verification-button"
                          onClick={() => handleAccept(seller.id, seller.userEmail)}
                        >
                          Accept
                        </button>
                      </td>
                      <td>
                        <button
                          className="verification-button"
                          onClick={() => handleDeny(seller.id, seller.userEmail)}
                        >
                          Deny
                        </button>
                      </td>
                    </>
                  ) : (
                    <>
                      <td>-</td>
                      <td>-</td>
                    </>
                  )}
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Verification;