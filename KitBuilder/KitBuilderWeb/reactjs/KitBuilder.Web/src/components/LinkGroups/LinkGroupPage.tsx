import * as React from 'react';
import { KbApiMethod } from '../helpers/kbapi'
import axios from 'axios';


interface ILinkGroupPageState {
    error: any,
    isLoaded: boolean,
    items: Array<any>,
    searchParamsLinkGroupName: string,
    searchParamsPLU: string
}

interface ILinkGroupPageProps {

}

export class LinkGroupPage extends React.Component<ILinkGroupPageProps, ILinkGroupPageState> {
    constructor(props: any) {
        super(props);
        this.state = {
            error: null,
            isLoaded: false,
            items: [],
            searchParamsLinkGroupName: '',
            searchParamsPLU: ''
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSearchClick = this.handleSearchClick.bind(this);
    }

    handleChange(event: any) {
        this.setState({ ...this.state, [event.target.name]: event.target.value });
    }

    handleSearchClick(event: any) {
        event.preventDefault();

        var url = KbApiMethod("LinkGroups");
        axios.get(url, {
            params: {
                SearchGroupNameQuery: this.state.searchParamsLinkGroupName,
                ScanCode: this.state.searchParamsPLU
            }
        })
            .then(
                (result) => {
                    console.log(result);
                    this.setState({ ...this.state, items: result.data });
                })
            .catch(function (error) {
                console.log(error);
            })
    }

    formatTable() {
        const items = this.state.items;
        const data = items.map((item) => 
        
        <li key={item.LinkGroupId}>{item.GroupName}</li>
        
        )
        return (
            <ul>{data}</ul>
        );
    }

    render() {
        return (
            <div>
                <h2>Link Groups</h2>

                <form>
                    <div className="form-row">
                        <div className="form-group col-md-5">
                            <label htmlFor="inputLinkGroupName">LinkGroup Name</label>
                            <input type="text"
                                className="form-control"
                                id="inputLinkGroupName"
                                placeholder="Link Group Name"
                                value={this.state.searchParamsLinkGroupName}
                                onChange={this.handleChange}
                                name="searchParamsLinkGroupName"
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="inputModifierPLU">Modifier PLU</label>
                            <input type="text" className="form-control" id="inputModifierPLU" placeholder="PLU"
                                value={this.state.searchParamsPLU}
                                onChange={this.handleChange}
                                name="searchParamsPLU"
                            />
                        </div>

                    </div>
                    <button id="btnSearch" className="btn btn-primary" onClick={this.handleSearchClick}>Search</button>



                </form>

                    {this.formatTable()}

                <pre> {JSON.stringify(this.state.items, null, 2)}</pre>

            </div>

        )
    }
}

export default LinkGroupPage