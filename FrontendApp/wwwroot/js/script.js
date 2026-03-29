const API = "https://medarbetarportal-userapi.azurewebsites.net";

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

    try {
        const response = await fetch(`${API}/api/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include", // credentials
            body: JSON.stringify({ email, password })
        });

        console.log("RESPONSE:", response);

        if (response.ok) {
            const user = await response.json();
            console.log(user);

            localStorage.setItem("user", JSON.stringify(user));

            if (user.role === "Admin") {
                window.location.href = "/Users/Admin";
            } else {
                document.getElementById("message").innerText = "Inloggad!";
            }

        } else {
            const text = await response.text();
            console.error("BACKEND ERROR:", text);
            document.getElementById("message").innerText = text;
        }

    } catch (err) {
        console.error("FETCH ERROR:", err);
        document.getElementById("message").innerText = "Nätverksfel";
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
        btn.disabled = false;
        return;
    }

    if (!emailInput.value.includes("@") || !emailInput.value.includes(".")) {
        message.style.color = "red";
        message.innerText = "Ogiltig email";
        btn.disabled = false;
        return;
    }

    try {
        const response = await fetch(`${API}/api/auth/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "include", 
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
            message.innerText = "Registrerad! Vänta på admin.";

            firstNameInput.value = "";
            lastNameInput.value = "";
            employeeIdInput.value = "";
            emailInput.value = "";
            passwordInput.value = "";

            setTimeout(() => {
                showLogin();
            }, 2000);

        } else {
            const text = await response.text();
            message.style.color = "red";
            message.innerText = text;
        }

    } catch (err) {
        console.error("REGISTER ERROR:", err);
        message.style.color = "red";
        message.innerText = "Något gick fel";
    }

    btn.disabled = false;
}