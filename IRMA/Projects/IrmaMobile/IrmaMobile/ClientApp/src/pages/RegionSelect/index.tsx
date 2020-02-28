import { WfmButton, WfmToggleGroup } from '@wfm/ui-react';
import React, { useContext, useEffect, useState } from 'react';
import { toast } from "react-toastify";
import agent from '../../api/agent';
import LoadingComponent from '../../layout/LoadingComponent';
import { AppContext, IShrinkAdjustment, IUser, types } from "../../store";
import './styles.scss';

const REGION_LIST = ['FL', 'MA', 'MW', 'NA', 'NC', 'NE', 'PN', 'RM', 'SO', 'SP', 'SW', 'EU'];
// @ts-ignore 
interface RegionProps {
	history: any;
}

const RegionSelect: React.FC<RegionProps> = (props) => {
	const { history } = props;
	// @ts-ignore 
	const { state, dispatch } = useContext(AppContext);
	const { isLoading } = state;
	const [loadingContent, setLoadingContent] = useState<string>("Loading stores...");

	useEffect(() => {
		dispatch({ type: types.RESETSTATE });
	}, [dispatch]);

	const setStores = (result: any) => {
		dispatch({ type: types.SETSTORES, stores: result });
	}

	const setIsLoading = (result: boolean) => {
		dispatch({ type: types.SETISLOADING, isLoading: result })
	}
	const setShrinkTypes = (result: any) => {
		dispatch({ type: types.SETSHRINKTYPES, shrinkTypes: result });
	}

	const setShrinkAdjustmentReasons = (result: IShrinkAdjustment[]) => {
		dispatch({ type: types.SETSHRINKADJUSTMENTREASONS, shrinkAdjustmentReasons: result });
	}

	const getStores = async (user: IUser) => {
		const { region } = state;
		try {
			setIsLoading(true);

			const stores = await agent.RegionSelect.getStores(region);
			const shrinkSubTypes = await agent.RegionSelect.getShrinkSubtypes(region);
			const shrinkAdjustmentReasons: IShrinkAdjustment[] = await agent.RegionSelect.getShrinkAdjustmentReasons(region);
			if (!stores) {
				dispatch({ type: types.SETREGION, region: '' });
				dispatch({ type: types.SETSTORE, store: '' });
				dispatch({ type: types.SETSTORES, store: [] });
				toast.error("Store could not be loaded. Try again.", { autoClose: false });
				return;
			}
			if (!shrinkSubTypes) {
				toast.error("Shrink Types could not be loaded. Try again.", { autoClose: false });
				return;
			}
			try {
				if (user.telxonStoreLimit === -1 || user.isSuperUser || user.isCoordinator) {
					setStores(stores);
				} else {
					setStores(stores.filter(s => parseInt(s.storeNo) === user.telxonStoreLimit));
				}
				setShrinkTypes(shrinkSubTypes);
				setShrinkAdjustmentReasons(shrinkAdjustmentReasons);
			} catch (err) {
				toast.error("Unable to Set Stores", { autoClose: false })
			}
		}
		finally {
			history.push('/store');
			setIsLoading(false);
		}
	}

	const handleSelectRegionClick = () => {
		setLoadingContent("Retrieving User information...");
		setIsLoading(true);
		dispatch({ type: types.SETUSER, user: null })
		const { region, subteamSession } = state;
		const getUser = async () => {
			try {
				const authToken = localStorage.getItem('authToken');
				if (authToken) {
					const parsedAuthToken = JSON.parse(authToken);
					const user = await agent.User.getUser(region, parsedAuthToken.samaccountname);
					if (!user) {
						toast.error("You are not authorized to use this application for this region.  Please contact your Regional IRMA Security Admin.");
						setIsLoading(false);
					} else if (!user.isAccountEnabled) {
						toast.error("Your IRMA account is disabled for this region.  Please contact your Regional IRMA Security Admin.");
						setIsLoading(false);
					} else {
						dispatch({ type: types.SETUSER, user: user });
					
						if ((subteamSession.filter((session: any) => session.sessionUser.userName === user.userName).length === 0)) {
							subteamSession.push({ shrinkItems: [], isPrevSession: false, sessionShrinkType: '', sessionNumber: 0, sessionSubteam: undefined, sessionStore: '', sessionRegion: '', sessionUser: user, forceSubteamSelection: true });
						}
						dispatch({ type: types.SETSUBTEAMSESSION, subteamSession });
						setLoadingContent("Loading Stores...");
						getStores(user);
					}
				} else {
					toast.error('Unable to load authorization token. Please reload the application and log back in.');
					setIsLoading(false);
				}
			} catch (error) {
				console.error(error);
				toast.error("Unable to load user information. Please close and re-open the application.");
				setIsLoading(false);
			}
		}
		getUser();
	}

	const selectRegion = (e: any) => {
		dispatch({ type: types.SETREGION, region: e.detail.value });
	}

	if (isLoading) {
		return (<LoadingComponent content={loadingContent} />)
	} else {
		return (
			<div>
				<div className="page-title-wrapper">
					<p>Select your Region</p>
				</div>
				<div className="region-container">
					<div className="toggle-group">
						<WfmToggleGroup buttons={REGION_LIST} direction="vertical" onWfmToggleButtonSelected={selectRegion} flex />
					</div>
				</div>
				<div className="region-select">
					<WfmButton disabled={state.region === ''} onClick={handleSelectRegionClick}>Select Region</WfmButton>
				</div>
			</div>)
	}
};

export default RegionSelect;