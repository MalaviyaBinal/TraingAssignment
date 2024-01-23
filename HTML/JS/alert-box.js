window.alert = function (message, timeout = null) {
    const alert = document.createElement('div');
    const alertButton = document.createElement('button');
    const alertIcon = document.createElement('button');
    const msg = document.createElement('p');
    const heading = document.createElement('p');
    alertButton.innerText = "OK";
    alertIcon.innerHTML = `<span style="padding:10px;"><i class="fa-solid fa-circle-exclamation" style="color: #ffa200; font-size : 80px"></i></span>`;
    msg.innerText = message;
    heading.innerText = "Information";
    alert.classList.add('alert');

    alert.setAttribute('style', `
        position : absolute;
        top : 150px;
        margin : 10px;
        max-width : 400px;
        max-height : 400px;
       
        background-color : #f3f3f3;
        padding : 20px;
        border-radius :10px;
        box-shadow : 0 10px 5px 0 #00000044;
        display : flex;
        flex-direction : column;
        backdrop-filter: blur(10px);
    
    `);
    alertIcon.setAttribute('style', `
        border : none;
    `);
    msg.setAttribute('style', `
        text-align : center;
    `);
    heading.setAttribute('style', `
        text-align : center;
        font-size : 20px;

    `);
    alertButton.setAttribute('style', `
    text-align : center;
    color : white;
    background-color : #54B4D3;
    border-color : #54B4D3;
    border-radius : 5px;
    padding : 5px;
    width : 50px;
    `);


    // alert.innerHTML = `<span style="padding:10px;"><i class="fa-solid fa-circle-exclamation fa-2xl" style="color: #ffa200;"></i></span>`;

    alert.appendChild(alertIcon);
    alert.appendChild(heading);
    alert.appendChild(msg);
    alert.appendChild(alertButton);



    alertButton.addEventListener('click', (e) => {
        alert.remove();

    });
    if (timeout != null) {
        setTimeout(() => {
            alert.remove();
        }, Number(timeout))
    }
    document.body.appendChild(alert);
}