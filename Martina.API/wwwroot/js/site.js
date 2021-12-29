
// Error: Se asigna el valor none al dropdown del menu para poder desplegarlo
// Inicio

document.getElementById('drop').style.display = 'none';

document.getElementById('navbardrop').addEventListener('click', () =>
{
    if (document.getElementById('drop').style.display == 'none')
    {
        expand();
    }
    else {
        collapse();
    }
})

function expand() {
    document.getElementById('drop').style.display = 'block';
}

function collapse() {
    document.getElementById('drop').style.display = 'none';
}

// Fin
