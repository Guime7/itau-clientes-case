-- Criação da tabela de clientes
-- Estrutura baseada em Itau.Case.Clientes.Domain.Entities.Cliente
CREATE TABLE IF NOT EXISTS Clientes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    Saldo DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    DataCriacao DATETIME NOT NULL,
    DataAtualizacao DATETIME NOT NULL,
    UNIQUE INDEX idx_email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Inserir alguns dados de exemplo
INSERT INTO Clientes (Nome, Email, Saldo, DataCriacao, DataAtualizacao) VALUES
('João Silva', 'joao.silva@email.com', 5000.00, UTC_TIMESTAMP(), UTC_TIMESTAMP()),
('Maria Santos', 'maria.santos@email.com', 7500.50, UTC_TIMESTAMP(), UTC_TIMESTAMP()),
('Pedro Oliveira', 'pedro.oliveira@email.com', 3200.75, UTC_TIMESTAMP(), UTC_TIMESTAMP());
