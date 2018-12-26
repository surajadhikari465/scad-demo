const ApiMethods  = {
    LinkGroups: "/api/LinkGroups",
     Kits:'/api/Kits',
     InstructionList: '/api/InstructionList/'
}

export function KbApiMethod(name: string) {
    var method = ApiMethods[name];
    console.log("KB API => "  + method);
    return process.env.REACT_APP_KB_API_URL +  method;
}