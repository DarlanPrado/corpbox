import { makeApi, Zodios, type ZodiosOptions } from "@zodios/core";
import { z } from "zod";

const RegisterRequest = z
  .object({
    name: z.string().min(2).max(120),
    email: z.string().min(0).max(255),
    password: z.string().min(8).max(100),
  })
  .strict()
  .passthrough();
const LoginRequest = z
  .object({
    email: z.string().min(0).max(255),
    password: z.string().min(8).max(100),
  })
  .strict()
  .passthrough();
const WeatherForecast = z
  .object({
    date: z.string(),
    temperatureC: z.union([z.number(), z.string()]),
    temperatureF: z.union([z.number(), z.string()]),
    summary: z.union([z.null(), z.string()]),
  })
  .partial()
  .strict()
  .passthrough();

export const schemas = {
  RegisterRequest,
  LoginRequest,
  WeatherForecast,
};

const endpoints = makeApi([
  {
    method: "post",
    path: "/api/Auth/login",
    alias: "postApiAuthlogin",
    requestFormat: "json",
    parameters: [
      {
        name: "body",
        type: "Body",
        schema: LoginRequest,
      },
    ],
    response: z.void(),
  },
  {
    method: "post",
    path: "/api/Auth/logout",
    alias: "postApiAuthlogout",
    requestFormat: "json",
    response: z.void(),
  },
  {
    method: "get",
    path: "/api/Auth/me",
    alias: "getApiAuthme",
    requestFormat: "json",
    response: z.void(),
  },
  {
    method: "post",
    path: "/api/Auth/refresh",
    alias: "postApiAuthrefresh",
    requestFormat: "json",
    response: z.void(),
  },
  {
    method: "post",
    path: "/api/Auth/register",
    alias: "postApiAuthregister",
    requestFormat: "json",
    parameters: [
      {
        name: "body",
        type: "Body",
        schema: RegisterRequest,
      },
    ],
    response: z.void(),
  },
  {
    method: "get",
    path: "/WeatherForecast",
    alias: "GetWeatherForecast",
    requestFormat: "json",
    response: z.array(WeatherForecast),
  },
]);

export const api = new Zodios(endpoints);

export function createApiClient(baseUrl: string, options?: ZodiosOptions) {
  return new Zodios(baseUrl, endpoints, options);
}
