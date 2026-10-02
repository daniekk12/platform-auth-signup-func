# GitHub → Render configuration (signup function)

See [Render API env sync](https://api-docs.render.com/reference/update-env-vars-for-service) and [trigger deploy](https://api-docs.render.com/reference/create-deploy).

Workflow: `.github/workflows/render-deploy.yml` + `.github/scripts/render_sync_deploy.py`.

## One-time Render setup

Create Docker Web Service from **`platform-auth-signup-func`**, copy **`srv-…`** from Settings. Deploy **first** in the platform sequence.

## GitHub Environment: `production`

Create environment **`production`**. Restrict deployment to `main` (and optional reviewers).

Deploy runs only when CI on **`main`** succeeds (`workflow_run`) or **`workflow_dispatch`** is run from **`main`**. Pull request CI does not sync production secrets to Render.

## Secrets

| Name | Purpose |
|------|---------|
| `RENDER_API_KEY` | Render API bearer token |
| `FunctionInvocation__ApiKey` | Same shared secret as gateway and login |

## Variables

| Name | Purpose |
|------|---------|
| `RENDER_SERVICE_ID` | This signup service’s `srv-…` ID |
| `PUBLIC_HEALTH_URL` | `https://<signup-host>/health` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `AllowedHosts__0` | `<signup-host>.onrender.com` |

Do not configure application env vars in the Render dashboard; GitHub is the source of truth.
