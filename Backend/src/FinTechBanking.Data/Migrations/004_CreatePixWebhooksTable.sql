-- Criar tabela pix_webhooks
CREATE TABLE IF NOT EXISTS pix_webhooks (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    event_type VARCHAR(100) NOT NULL,
    webhook_url VARCHAR(500) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    retry_count INTEGER NOT NULL DEFAULT 0,
    last_attempt TIMESTAMP NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,
    CONSTRAINT fk_pix_webhooks_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Criar índices
CREATE INDEX IF NOT EXISTS idx_pix_webhooks_user_id ON pix_webhooks(user_id);
CREATE INDEX IF NOT EXISTS idx_pix_webhooks_event_type ON pix_webhooks(event_type);
CREATE INDEX IF NOT EXISTS idx_pix_webhooks_is_active ON pix_webhooks(is_active);
CREATE INDEX IF NOT EXISTS idx_pix_webhooks_user_event ON pix_webhooks(user_id, event_type);

