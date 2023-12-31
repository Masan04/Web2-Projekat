export const userModel = ({
    id: 0,
    username: '',
    password: '',
    useremail: '',
    name: '',
    surname: '',
    date: '',
    address: '',
    type: -1,
    picture: '',
    status: -1,
  });

export const userLoginModel = ({
    email: '',
    password: '',
  });

export const GetToken =  () =>
{
    return localStorage.getItem('token');
}

export const SetToken =  (token) =>
{
    localStorage.setItem('token', token);
}

export const GetRole =  () =>
{
    return localStorage.getItem('role');
}

export const SetRole =  (role) =>
{
    localStorage.setItem('role', role["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);        
}

export const GetEmail =  () =>
{
    return localStorage.getItem('email');
}

export const SetEmail =  (email) =>
{
    localStorage.setItem('email', email);
}

export const SetUser =  (user) =>
{
    localStorage.setItem('user', JSON.stringify(user));
}

export const GetUser =  () =>
{
    return JSON.parse(localStorage.getItem('user'));
}

export const GetStatus = () => 
{
    return JSON.parse(localStorage.getItem('status'));
}

export const SetStatus = (status) =>
{
    localStorage.setItem('status', status["http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata"])
}