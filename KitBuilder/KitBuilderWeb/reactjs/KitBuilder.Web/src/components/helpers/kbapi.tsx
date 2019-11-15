const ApiMethods  = {
    LinkGroups: "/api/LinkGroups",
     Kits: "/api/Kits",
     InstructionList: "/api/InstructionList/",
     LinkGroupsSearch: "/api/LinkGroups/LinkGroupsSearch",
     AssignKit: "/api/Venues",
     InstructionListByType: "/api/InstructionList/GetInstructionsListByType",
     Items: "/api/Items/",
     LinkGroupItemSearch:"/api/LinkGroups/LinkGroupItemSearch",
     LocalesByType: "/api/Locales/GetLocaleByType",
     Locales:"api/Locales", 
     LocalesSearch: "api/Locales/GetLocales",
     Login: "api/Authentication/Login"
}

export function KbApiMethod(name: string) {
    var method = ApiMethods[name];;
    return process.env.REACT_APP_KB_API_URL +  method;
}