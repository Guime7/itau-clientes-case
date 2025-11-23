# Dockerfile for development
FROM node:20-alpine

# Define argumentos para configuração de porta
ARG PORT=4200
ENV PORT=${PORT}

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY . .

# Expose configurable port
EXPOSE ${PORT}

# Start development server
CMD ["sh", "-c", "npm start -- --host 0.0.0.0 --port ${PORT} --poll 2000"]
