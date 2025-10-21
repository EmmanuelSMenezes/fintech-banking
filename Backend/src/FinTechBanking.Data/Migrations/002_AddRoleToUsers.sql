-- Add role column to users table
ALTER TABLE users ADD COLUMN IF NOT EXISTS role VARCHAR(50) DEFAULT 'user';

-- Update existing admin user to have admin role
UPDATE users SET role = 'admin' WHERE email = 'admin@owaypay.com';

