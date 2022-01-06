FROM node:14 as build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY /package.json /app/package.json
COPY /.npmrc /app/.npmrc
RUN npm install react-scripts
RUN npm install
COPY . /app
ARG ENV=development
RUN npm run build:$ENV

FROM nginx
COPY --from=build /app/build /usr/share/nginx/html
RUN touch /usr/share/nginx/html/healthcheck
EXPOSE 80
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]