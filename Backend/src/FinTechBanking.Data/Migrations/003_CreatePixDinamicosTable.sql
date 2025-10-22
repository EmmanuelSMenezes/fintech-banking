-- Create pix_dinamicos table
CREATE TABLE IF NOT EXISTS pix_dinamicos (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    account_id UUID NOT NULL REFERENCES accounts(id) ON DELETE CASCADE,
    amount DECIMAL(18, 2) NOT NULL,
    description VARCHAR(500) NOT NULL,
    recipient_key VARCHAR(255) NOT NULL,
    qr_code_data TEXT NOT NULL,
    qr_code_url VARCHAR(500) NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'PENDING',
    external_id VARCHAR(255),
    bank_code VARCHAR(10) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP,
    paid_at TIMESTAMP,
    updated_at TIMESTAMP
);

-- Create indexes for better query performance
CREATE INDEX IF NOT EXISTS idx_pix_dinamicos_user_id ON pix_dinamicos(user_id);
CREATE INDEX IF NOT EXISTS idx_pix_dinamicos_account_id ON pix_dinamicos(account_id);
CREATE INDEX IF NOT EXISTS idx_pix_dinamicos_status ON pix_dinamicos(status);
CREATE INDEX IF NOT EXISTS idx_pix_dinamicos_created_at ON pix_dinamicos(created_at);

