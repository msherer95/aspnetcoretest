${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;

    Template(Settings settings)
    {
        settings.OutputExtension = ".d.ts";
    }

} $Classes(*Models.*)[   interface $Name { $Properties[
        $Name?: $Type;]   
    }]