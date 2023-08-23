import axios from "axios";
import { GetEmail, GetRole, GetToken, GetStatus } from "../models/UserModel";


export const config =
{
    headers: {
        "Authorization" : `Bearer ${GetToken()}`
    }
};

export const AddUser = async (account) =>
{
    return await axios.post('https://localhost:7293' + '/api/User/register', account);
}

export const LoginUser = async (account) =>
{
    return await axios.post('https://localhost:7293' + '/api/User/login', account);
}

export const LoginGoogle = async (account) =>
{
    return await axios.post(process.env.REACT_APP_API_URL + '/api/User/loginGoogle', account);
}

export const GetUserFromBackend = async () =>
{   
    const email = GetEmail();
    return await axios.get(process.env.REACT_APP_API_URL + '/api/User/' + email, config);
}

export const GetUserAfterLogin = async (token) =>
{   
    const email = GetEmail();
    const firstConfig = {  headers: {"Authorization" : `Bearer ${token}`} };
    return await axios.get(process.env.REACT_APP_API_URL + '/api/User/' + email, firstConfig);
}

export const GetAllUsers = async () =>
{   
    return await axios.get(process.env.REACT_APP_API_URL + '/api/User/all', config);
}

export const UpdateUser = async (id, account) =>
{
    return await axios.put(process.env.REACT_APP_API_URL + '/api/User/' + id, account, config);
}

export const GetUserById = async (id) =>
{   
    return await axios.get(process.env.REACT_APP_API_URL + '/api/User/id/' + id, config);
}
export const GetVerification = async (isVerified) =>
{   
    const email = GetEmail();
    return await axios.get(process.env.REACT_APP_API_URL + '/api/User/verify/' + email, isVerified);
}


export const AuthUser = () => 
{
    const token = GetToken();
    if(token === null)
        return null;
    else
    {
    
        const role = GetRole();
        // if(role === "seller")
        //     return null;
        return role;
    }
}