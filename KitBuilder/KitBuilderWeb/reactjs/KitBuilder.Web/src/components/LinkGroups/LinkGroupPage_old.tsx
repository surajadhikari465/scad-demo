import * as React from 'react';
import { KbApiMethod } from '../helpers/kbapi'
import ReactTable from 'react-table'
import 'react-table/react-table.css'




import axios from 'axios';


interface ILinkGroupPageState {
    error: any,
    isLoaded: boolean,
    items: Array<any>,
    searchParamsLinkGroupName: string,
    searchParamsPLU: string,
    showAddLinkGroup: boolean
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
            searchParamsPLU: '',
            showAddLinkGroup: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSearchClick = this.handleSearchClick.bind(this);
        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
    }

    handleCloseModal() {
        this.setState({ ...this.state, showAddLinkGroup: false });
    }

    handleOpenModal() {
        this.setState({ ...this.state, showAddLinkGroup: true });
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
                ScanCode: this.state.searchParamsPLU,
                PageSize: 10
            }
        })
            .then(
                (result) => {
                    console.log(result.data);
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

    returnColumns() {
        const columns = [{
            Header: 'Id',
            accessor: 'LinkGroupId'
        },
        {
            Header: 'Group Name',
            accessor: 'GroupName'
        },
        {
            Header: 'Description',
            accessor: 'GroupDescription'
        }]
            ;

        return columns;
    }

    

    render() {
        return (
            <div>

                
                <h2>Link Groups</h2>
                <button className="btn btn-success" onClick={this.handleOpenModal}>Add</button>
            
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

                <div>
                    {
                        this.state.items.length > 0 && <ReactTable
                            data={this.state.items}
                            columns={this.returnColumns()}
                            defaultPageSize={10}
                            className="-striped -highlight"
                        />
                    }
                </div>


            </div>


        )
    }
}

export default LinkGroupPage
