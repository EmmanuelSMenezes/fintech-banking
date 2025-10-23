-- Criar tabela de transferências agendadas
CREATE TABLE IF NOT EXISTS scheduled_transfers (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    account_id UUID NOT NULL,
    recipient_key VARCHAR(255) NOT NULL,
    amount DECIMAL(18, 2) NOT NULL,
    description TEXT,
    scheduled_date TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'PENDING',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    executed_at TIMESTAMP,
    error_message TEXT,
    CONSTRAINT fk_scheduled_transfers_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    CONSTRAINT fk_scheduled_transfers_account FOREIGN KEY (account_id) REFERENCES accounts(id) ON DELETE CASCADE
);

-- Criar índices para melhor performance
CREATE INDEX idx_scheduled_transfers_user_id ON scheduled_transfers(user_id);
CREATE INDEX idx_scheduled_transfers_status ON scheduled_transfers(status);
CREATE INDEX idx_scheduled_transfers_scheduled_date ON scheduled_transfers(scheduled_date);
CREATE INDEX idx_scheduled_transfers_user_status ON scheduled_transfers(user_id, status);
CREATE INDEX idx_scheduled_transfers_pending_date ON scheduled_transfers(scheduled_date) WHERE status = 'PENDING';

