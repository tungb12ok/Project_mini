-- Insert sample data into roles
INSERT INTO roles (name, description)
VALUES
('Admin', 'Administrator with full access'),
('Manager', 'Department manager with specific access'),
('Staff', 'Regular staff member with limited access');

-- Insert sample data into departments
INSERT INTO departments (name, parent_id)
VALUES
('Head Office', NULL),
('IT Department', 1),
('HR Department', 1),
('Finance Department', 1),
('Marketing Department', 1),
('Development Team', 2),
('Support Team', 2);

-- Insert sample data into users
INSERT INTO users (name, email, password, role_id, department_id)
VALUES
('John Doe', 'john.doe@example.com', 'password123', 1, 1),  -- Admin in Head Office
('Jane Smith', 'jane.smith@example.com', 'password456', 2, 2),  -- Manager in IT Department
('Alice Johnson', 'alice.johnson@example.com', 'password789', 3, 3),  -- Staff in HR Department
('Bob Lee', 'bob.lee@example.com', 'passwordABC', 2, 2),  -- Manager in Finance Department
