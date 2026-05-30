-- Script de criação do banco de dados - Vendinha Plena
-- Banco: SQLite

CREATE TABLE IF NOT EXISTS Clientes (
    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome          TEXT    NOT NULL,
    CPF           TEXT    NOT NULL UNIQUE,
    DataNascimento TEXT   NOT NULL,
    Email         TEXT
);

CREATE TABLE IF NOT EXISTS Dividas (
    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
    ClienteId      INTEGER NOT NULL,
    Valor          REAL    NOT NULL,
    Paga           INTEGER NOT NULL DEFAULT 0,  -- 0 = não paga, 1 = paga
    DataCriacao    TEXT    NOT NULL,
    DataPagamento  TEXT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
