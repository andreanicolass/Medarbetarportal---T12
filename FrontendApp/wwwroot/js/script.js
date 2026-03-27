function showRegister() {
    document.getElementById("loginBox").style.display = "none";
    document.getElementById("registerBox").style.display = "block";
}

function showLogin() {
    document.getElementById("loginBox").style.display = "block";
    document.getElementById("registerBox").style.display = "none";
}

// LOGIN
async function login() {
    console.log("LOGIN KLICKAD");
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    const response = await fetch("http://localhost:5094/api/auth/login", {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
    });
    console.log("RESPONSE:", response);

    if (response.ok) {
        const user = await response.json();
        console.log(user);

        localStorage.setItem("user", JSON.stringify(user));

        // redirect admin
        if (user.role=== "Admin") {
            window.location.href = "/Users/Admin";
        } else {
            document.getElementById("message").innerText = "Inloggad!";
        }

    } else {
        document.getElementById("message").innerText = "Fel login";
    }
}

// REGISTER
async function register() {
    const firstNameInput = document.getElementById("regFirstName");
    const lastNameInput = document.getElementById("regLastName");
    const employeeIdInput = document.getElementById("regEmployeeId");
    const emailInput = document.getElementById("regEmail");
    const passwordInput = document.getElementById("regPassword");

    const message = document.getElementById("message");
    const btn = document.querySelector("#registerBox button");

    // disable knapp
    btn.disabled = true;

    if (
        !firstNameInput.value ||
        !lastNameInput.value ||
        !employeeIdInput.value ||
        !emailInput.value ||
        !passwordInput.value
    ) {
        message.style.color = "red";
        message.innerText = "Alla fält måste fyllas i korrekt";
        return;
    }

    const email = emailInput.value;

    if (!email.includes("@") || !email.includes(".")) {
        message.style.color = "red";
        message.innerText = "Ogiltig email";
        return;
    }

    if (!firstNameInput.value) {
        message.innerText = "Förnamn saknas";
        return;
    }

    try {
        const response = await fetch("http://localhost:5094/api/auth/register", {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                firstName: firstNameInput.value,
                lastName: lastNameInput.value,
                employeeId: employeeIdInput.value,
                email: emailInput.value,
                password: passwordInput.value
            })
        });

        if (response.ok) {
            message.style.color = "green";
            message.style.opacity = "1";
            message.innerText = "Registrerad! Vänta på admin.";

            // töm fält
            firstNameInput.value = "";
            lastNameInput.value = "";
            employeeIdInput.value = "";
            emailInput.value = "";
            passwordInput.value = "";

            // tillbaka till login
            setTimeout(() => {
                showLogin();
            }, 3000);

        } else {
            const text = await response.text();
            message.style.color = "red";
            message.style.opacity = "1";
            message.innerText = text;
        }

    } catch (err) {
        console.error(err);
        message.style.color = "red";
        message.innerText = "Något gick fel";
    }

    btn.disabled = false;

    // fade ut
    setTimeout(() => {
        message.style.opacity = "0";
    }, 3000);

    setTimeout(() => {
        message.innerText = "";
        message.style.opacity = "1";
    }, 3000);
}