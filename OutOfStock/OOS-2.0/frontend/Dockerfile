FROM node as build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY /package.json /app/package.json
COPY /.npmrc /app/.npmrc
RUN npm install
RUN npm install -g @angular/cli
COPY . /app
ARG ENV=dev
RUN ng build -c $ENV

FROM nginx
COPY --from=build /app/dist/outofstock /usr/share/nginx/html
RUN touch /usr/share/nginx/html/healthcheck
EXPOSE 80
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]