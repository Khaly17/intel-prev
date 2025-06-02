export function get(cname)
{
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

export function set(cname, cvalue, exminutes)
{
    // sets the cookie key-value pair
    const d = new Date();
    d.setTime(d.getTime() + (exminutes*60*1000));
    let expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    
    
    
    
    //if key  is token set value
    // if(key === 'token')
    // {
    //     document.cookie = value;
    // }
    // document.cookie=`${key}=${value}`;
    
}