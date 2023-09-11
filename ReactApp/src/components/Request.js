import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { GetBasket } from "../models/ItemModel";

const Request = () => {
    const history = useNavigate();

    useEffect( () => {
        const itemsOrder = GetBasket();
        console.log(itemsOrder);
        if(itemsOrder !== null){
            if(itemsOrder.length !== 0)
                history(`/napraviPorudzbinu/${encodeURIComponent(JSON.stringify(itemsOrder))}`);
        }
    })

    return ( 
        <div className="empty-request">
        <h2>Vasa korpa je prazna!</h2>
        </div>
     );
}
 
export default Request;