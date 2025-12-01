# STAGE 1: Build Angular
FROM node:18-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration production

# STAGE 2: Servir con Nginx
FROM nginx:alpine AS final
COPY --from=build /app/dist/chubb.front/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]