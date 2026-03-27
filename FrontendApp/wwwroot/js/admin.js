const API = "https://medarbetarportal-ajgsfkg4gug3bpbb.polandcentral-01.azurewebsites.net"
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
            throw new Error("API svarade inte korrekt");
        }

        users = await u.json();
        roles = await r.json();
        departments = await d.json();

        console.log("Users:", users);

        render(users);

    } catch (err) {
        console.error("Fel vid laddning:", err);
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
        await fetch(`${API}/api/auth/approve/${id}`, {
            method: "PUT",
            credentials: "include"
        });

        loadData();

    } catch (err) {
        console.error("Approve error:", err);
    }
}

async function save(id) {
    const roleId = document.getElementById(`role-${id}`).value;
    const depId = document.getElementById(`dep-${id}`).value;

    try {
        await fetch(`${API}/api/users/${id}`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                roleId: parseInt(roleId),
                departmentId: parseInt(depId)
            })
        });

        alert("Ändringar sparade");

    } catch (err) {
        console.error("Save error:", err);
    }
}

async function deleteUser(id) {
    if (!confirm("Är du säker på att du vill ta bort användaren?")) return;

    try {
        await fetch(`${API}/api/users/${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        loadData();

    } catch (err) {
        console.error("Delete error:", err);
    }
}

function filterPending() {
    render(users.filter(u => !u.isApproved));
}

window.onload = loadData;
