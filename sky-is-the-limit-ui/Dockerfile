FROM nginx:1.23.3

ENV VERSION="1.0.0-SNAPSHOT"

EXPOSE 80

ENV API_URL http://localhost:8080/api

ADD dist/sky-is-the-limit-ui/browser /usr/share/nginx/html/
ADD docker/nginx/templates/ /etc/nginx/templates

