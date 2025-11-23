# MySQL configured to simulate AWS RDS Aurora
FROM mysql:8.0

# Set environment variables for MySQL
ENV MYSQL_ROOT_PASSWORD=root_password
ENV MYSQL_DATABASE=case_clientes
ENV MYSQL_USER=case_user
ENV MYSQL_PASSWORD=case_password

# Aurora-like configuration
# Enable InnoDB and optimize for performance
RUN echo "[mysqld]" > /etc/mysql/conf.d/aurora-like.cnf && \
    echo "default-storage-engine=InnoDB" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "innodb_buffer_pool_size=256M" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "innodb_log_file_size=64M" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "innodb_flush_log_at_trx_commit=2" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "innodb_flush_method=O_DIRECT" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "max_connections=150" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "character-set-server=utf8mb4" >> /etc/mysql/conf.d/aurora-like.cnf && \
    echo "collation-server=utf8mb4_unicode_ci" >> /etc/mysql/conf.d/aurora-like.cnf

# Copy initialization scripts
COPY tables/*.sql /docker-entrypoint-initdb.d/

# Expose MySQL port
EXPOSE 3306
