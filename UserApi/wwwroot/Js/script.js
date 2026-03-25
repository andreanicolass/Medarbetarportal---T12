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
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    const response = await fetch("http://localhost:5094/api/auth/login", {
        method: "POST",
        credentials: "include", 
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
    });

    if (response.ok) {
        const user = await response.json();

        localStorage.setItem("user", JSON.stringify(user));

        // redirect admin
        if (user.role === "Admin") {
            window.location.href = "admin.html";
        } else {
            document.getElementById("message").innerText = "Inloggad!";
        }

    } else {
        document.getElementById("message").innerText = "Fel login";
    }
}

// 📝 REGISTER
async function register() {
    const firstName = document.getElementById("regFirstName").value;
    const lastName = document.getElementById("regLastName").value;
    const employeeId = document.getElementById("regEmployeeId").value;
    const email = document.getElementById("regEmail").value;
    const password = document.getElementById("regPassword").value;

    const response = await fetch("http://localhost:5094/api/auth/register", {
        method: "POST",
        credentials: "include", 
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            firstName,
            lastName,
            employeeId,
            email,
            password
        })
    });

    if (response.ok) {
        document.getElementById("message").innerText =
            "Registrerad! Vänta på admin.";
    } else {
        document.getElementById("message").innerText =
            "Registrering misslyckades";
    }
}