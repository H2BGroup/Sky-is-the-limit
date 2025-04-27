db = db.getSiblingDB('admin');

db.auth("root", "example");

db = db.getSiblingDB("PaymentsDB");

db.createUser(
    {
        user: "user",
        pwd: "password",
        roles: [
            {
                role: "readWrite",
                db: "PaymentsDB"
            }
        ]
    }
);

db.createCollection("Payments");