import axios from 'axios'
import { KbApiMethod } from '../helpers/kbapi';

export function PerformLinkGroupSearch(groupName: string, groupDesc: string, modifierName: string, modifierPlu: string) {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("LinkGroupsSearch"), {
            params: {
                LinkGroupName: groupName,
                LinkGroupDesc: groupDesc,
                ModifierName: modifierName,
                ModifierPLU: modifierPlu
            }
        }).then(res => {
            resolve(res.data);
        }).catch(error => {
            reject(error);
        });
    })
}

export function PerformLinkGroupModifierSearch(modifierPlu: string, modifierName: string) {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("LinkGroupItemSearch"), {
            params: {
                ModiferPlu: modifierPlu,
                ModifierName: modifierName
            }
        }).then(res => {
            resolve(res.data);
        }).catch(error => {
            reject(error);
        });
    })
}

export function LoadCookingInstructions() {
    return new Promise((resolve, reject) => {
        axios.get(KbApiMethod("InstructionListByType"), {
            params: {
                instructionListType: "Cooking"
            }
        })
            .then(res => {
                resolve(res.data)
            }).catch(error => {
                reject(error)
            });
    })
}