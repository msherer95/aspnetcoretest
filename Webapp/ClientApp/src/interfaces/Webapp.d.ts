declare namespace Webapp {

    interface ApplicationRole {    
    }

    interface ApplicationUser { 
        FirstName?: string;
        LastName?: string;   
    }

    interface LoginUser { 
        UserName?: string;
        Password?: string;   
    }

    interface MappingProfile {    
    }

    interface RegistrationUser { 
        FirstName?: string;
        LastName?: string;
        Email?: string;
        UserName?: string;
        Password?: string;   
    }

    interface TestModel1 { 
        SomeBool?: boolean[];
        SomeBool2?: boolean;
        model2?: TestModel2;   
    }

    interface TestModel2 { 
        SomeInt1234987?: number;
        SomeString?: string;   
    }

}
