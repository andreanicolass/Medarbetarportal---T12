const API = "https://medarbetarportal-userapi.azurewebsites.net";

let users = [];
let roles = [];
let departments = [];

async function loadData() {
    try {
        console.log("Laddar Data...");

        const [u, r, d] = await Promise.all([
            fetch(`${API}/api/users`, { credentials: "include" }),
            fetch(`${API}/api/users/roles`, { credentials: "include" }),
            fetch(`${API}/api/users/departments`, { credentials: "include" })
        ]);

        if (!u.ok || !r.ok || !d.ok) {
            if (u.status === 401 || r.status === 401 || d.status === 401) {
                throw new Error("Inte inloggad (401)");
            }
            throw new Error("API svarade inte korrekt");
        }

        users = await u.json();
        roles = await r.json();
        departments = await d.json();

        console.log("Users:", users);

        render(users);

    } catch (err) {
        console.error("Fel vid laddning:", err);

        if (err.message.includes("401")) {
            alert("Du är inte inloggad. Logga in igen.");
            window.location.href = "/"; // tillbaka till login
        }
    }
}

function render(data) {
    const container = document.getElementById("usersContainer");
    container.innerHTML = "";

    if (!data || data.length === 0) {
        container.innerHTML = "<p>Inga användare hittades</p>";
        return;
    }

    data.forEach(user => {

        const fullName = user.name || `${user.firstName || ""} ${user.lastName || ""}`;

        const card = document.createElement("div");
        card.className = "card";

        card.innerHTML = `
            <h3>${fullName}</h3>
            <p>${user.email}</p>

            <div class="status ${user.isApproved ? "status-approved" : "status-pending"}">
                ${user.isApproved ? "Godkänd" : "Väntar på godkännande"}
            </div>

            <label class="label">Roll</label>
            <select id="role-${user.id}">
                ${roles.map(r => `
                    <option value="${r.id}" ${user.role?.name === r.name ? "selected" : ""}>
                        ${r.name}
                    </option>
                `).join("")}
            </select>

            <label class="label">Avdelning</label>
            <select id="dep-${user.id}">
                ${departments.map(d => `
                    <option value="${d.id}" ${user.department?.name === d.name ? "selected" : ""}>
                        ${d.name}
                    </option>
                `).join("")}
            </select>

            ${
            !user.isApproved
                ? `
                    <div class="actions">
                        <button class="btn-primary" onclick="approve(${user.id})">
                            Godkänn
                        </button>
                        <button class="btn-delete" onclick="deleteUser(${user.id})">
                            Ta bort
                        </button>
                    </div>
                  `
                : `
                    <div class="actions">
                        <button class="btn-save" onclick="save(${user.id})">
                            Spara
                        </button>
                        <button class="btn-delete" onclick="deleteUser(${user.id})">
                            Ta bort
                        </button>
                    </div>
                  `
        }
        `;

        container.appendChild(card);
    });
}

async function approve(id) {
    try {
        const res = await fetch(`${API}/api/auth/approve/${id}`, {
            method: "PUT",
            credentials: "include"
        });

        if (!res.ok) {
            const text = await res.text();
            throw new Error(text);
        }

        loadData();

    } catch (err) {
        console.error("Approve error:", err);
        alert("Fel vid godkännande");
    }
}

async function save(id) {
    const roleId = document.getElementById(`role-${id}`).value;
    const depId = document.getElementById(`dep-${id}`).value;

    try {
        const res = await fetch(`${API}/api/users/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                roleId: parseInt(roleId),
                departmentId: parseInt(depId)
            })
        });

        if (!res.ok) {
            const text = await res.text();
            throw new Error(text);
        }

        alert("Ändringar sparade");

    } catch (err) {
        console.error("Save error:", err);
        alert("Fel vid sparande");
    }
}

async function deleteUser(id) {
    if (!confirm("Är du säker på att du vill ta bort användaren?")) return;

    try {
        const res = await fetch(`${API}/api/users/${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        if (!res.ok) {
            const text = await res.text();
            throw new Error(text);
        }

        loadData();

    } catch (err) {
        console.error("Delete error:", err);
        alert("Fel vid borttagning");
    }
}

function filterPending() {
    render(users.filter(u => !u.isApproved));
}

window.onload = loadData;