  INSERT INTO [multiTenantAppDb].[dbo].[Tenants]
  ([Id],[Name],[ConnectionString])
    VALUES ('gamma', 'Gamma Group',
     'Server=localhost,1433;Database=GammaGroupDb;User Id=SA;Password=MotDePasseFort123!;Encrypt=True;TrustServerCertificate=True;Connect Timeout=30;MultipleActiveResultSets=True;'
    )