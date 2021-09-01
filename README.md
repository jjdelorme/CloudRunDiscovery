# Cloud Run Service Discovery

This demonstrates a simple method for determining the fully qualified public URL of a peer Cloud Run service that is deployed in the same project & region.

## Building the application

This sample application relies on [Cloud Run deploy from source](https://cloud.google.com/run/docs/deploying-source-code).  Which will use the LTS .NET 3.1 cloud native build pack.

In this repository you will find a `cloudbuild.yaml` file which defines a Cloud Build which automates the deployment to Cloud Run.

Make sure to include the environment variable `ASPNETCORE_URLS=http://0.0.0.0:8080` in your Cloud Run definition as this is tells ASP.NET to listen on port 8080 which is the default port for Cloud Run.

```yaml
  - '--update-env-vars'
  - 'ASPNETCORE_URLS=http://0.0.0.0:8080'
```

## Known Issues

If you see an error like this:
```
ERROR: (gcloud.run.deploy) PERMISSION_DENIED: Permission 'artifactregistry.repositories.create' denied on resource ...
```

You _may_ need to manually run the `gcloud run deploy [your-service-name] --source .` command once in your project prior to automating this with Cloud Build.  The reason is that Cloud Run uses an Artifact Repository named `cloud-run-source-deploy`.  Cloud Build by default has permissions to read/write to this repository, but not necessarily to create the repository.