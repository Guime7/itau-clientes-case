-- Criação da tabela de transações
-- Estrutura baseada em Itau.Case.Clientes.Domain.Entities.Transacao
-- ETipoTransacao: Deposito = 1, Saque = 2
CREATE TABLE IF NOT EXISTS Transacoes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ClienteId INT NOT NULL,
    Tipo INT NOT NULL COMMENT '1=Deposito, 2=Saque',
    Valor DECIMAL(18, 2) NOT NULL,
    Descricao VARCHAR(500) NULL,
    DataTransacao DATETIME NOT NULL,
    INDEX idx_cliente_id (ClienteId),
    INDEX idx_data_transacao (DataTransacao),
    INDEX idx_tipo (Tipo),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Inserir algumas transações de exemplo
INSERT INTO Transacoes (ClienteId, Tipo, Valor, Descricao, DataTransacao) VALUES
(1, 1, 5000.00, 'Depósito inicial', UTC_TIMESTAMP()),
(2, 1, 7500.50, 'Depósito inicial', UTC_TIMESTAMP()),
(3, 1, 3200.75, 'Depósito inicial', UTC_TIMESTAMP()),
(1, 2, 150.00, 'Saque no caixa eletrônico', UTC_TIMESTAMP()),
(2, 1, 500.00, 'Depósito via transferência', UTC_TIMESTAMP());
