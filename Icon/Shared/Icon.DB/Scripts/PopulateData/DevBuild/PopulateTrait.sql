print N'Populating Trait...'

SET IDENTITY_INSERT [dbo].[Trait] ON 

GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (0, N'PRD', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$', N'Product Description', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (1, N'POS', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,25}$', N'POS Description', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (2, N'PKG', N'^[0-9]*\.?[0-9]+$', N'Package Unit', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (3, N'FSE', N'0|1', N'Food Stamp Eligible', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (4, N'SCT', N'0|1', N'POS Scale Tare', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (5, N'DPT', N'0|1', N'Department Sale', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (6, N'GFT', N'0|1', N'Gift Card', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (7, N'RSZ', N'^[0-9]*\.?[0-9]+$', N'Retail Size', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (8, N'RUM', N'^[a-zA-z ]+$', N'Retail UOM', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (9, N'TMD', N'0|1', N'TM Discount Eligible', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (10, N'CSD', N'0|1', N'Case Discount Eligible', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (11, N'PRH', N'0|1', N'Prohibit Discount', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (12, N'AGE', N'^[0-9]*\.?[0-9]+$', N'Age Restrict', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (13, N'RCL', N'0|1', N'Recall', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (14, N'RES', N'^[0-9]*\.?[0-9]+$', N'Restricted Hours', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (15, N'SBW', N'0|1', N'Sold by Weight', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (16, N'FCT', N'0|1', N'Force Tare', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (17, N'QTY', N'0|1', N'Quantity Required', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (18, N'PRQ', N'0|1', N'Price Required', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (19, N'QPR', N'^[0-9]*\.?[0-9]+$', N'Quantity Prohibit', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (20, N'VV', N'0|1', N'Visual Verify', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (21, N'RS', N'0|1', N'Locked For Sale', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (22, N'NA', N'0|1', N'Authorized For Sale', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (23, N'DEL', N'0|1', N'Delete', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (24, N'LOC', N'^[0-9]*\.?[0-9]+$', N'Locale', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (25, N'PRC', N'^[0-9]*\.?[0-9]+$', N'Price', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (26, N'PM', N'^[0-9]*\.?[0-9]+$', N'Price Multiple', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (27, N'ST', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price Start Date', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (28, N'END', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Price End Date', 3)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (29, N'SHT', N'^[a-zA-Z0-9_]*$', N'Short Romance', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (30, N'LNG', N'^[a-zA-Z0-9_]*$', N'Long Romance', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (31, N'GF', N'0|1', N'Gluten Free', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (32, N'PBC', N'0|1', N'Premium Body Care', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (33, N'EX', N'0|1', N'Exclusive', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (34, N'WT', N'0|1', N'Whole Trade', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (35, N'NGM', N'0|1', N'Non GMO', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (36, N'HSH', N'0|1', N'HSH', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (37, N'E2', N'0|1', N'E2', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (38, N'VGN', N'0|1', N'Vegan', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (39, N'VEG', N'0|1', N'Vegetarian', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (40, N'KSH', N'0|1', N'Kosher', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (41, N'ECO', N'Baseline|Premium|Ultra-Premium', N'ECO Scale Rating', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (42, N'OG', N'0|1', N'Organic', 4)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (43, N'ABB', N'^[a-zA-Z0-9_]*$', N'Region Abbreviation', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (44, N'BU', N'^[a-zA-Z0-9_]*$', N'PS Business Unit ID', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (45, N'VER', N'^[0-9]*\.?[0-9]+$', N'ScanCode Version', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (46, N'INS', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Insert Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (47, N'MOD', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Modified Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (48, N'VAL', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Validation Date', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (49, N'MFM', N'^[a-zA-Z0-9_]*$', N'Merch Fin Mapping', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (50, N'GL', N'^[a-zA-Z0-9_]*$', N'GL Account', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (51, N'ABR', N'^[\d]{7} [\w \-\\/%<>&=\+]+$', N'Tax Abbreviation', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (52, N'SBC', N'^[a-zA-Z0-9_]*$', N'Sub Brick Code', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (53, N'FIN', N'^[a-zA-Z0-9_]*$', N'Financial Hierarchy Code', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (54, N'ESB', N'^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', N'Sent To ESB', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (55, N'PHN', N'^[0-9]{10,11}$', N'Phone Number', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (56, N'CPN', N'^[a-zA-Z0-9_]*$', N'Contact Person', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (57, N'SAB', N'^[a-zA-Z0-9_]*$', N'Store Abbreviation', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (58, N'NM', N'Bottle Deposit|CRV|Coupon|Bottle Return|CRV Credit|Legacy POS Only|Blackhawk Fee|Non-Retail', N'Non-Merchandise', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (59, N'LSC', N'^[0-9]*\.?[0-9]+$', N'Linked Scan Code', 2)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (60, N'USR', N'^[a-zA-Z0-9_]*$', N'Modified User', 6)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (61, N'AFF', N'1', N'Affinity', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (62, N'PDN', N'^[0-9_]*$', N'POS Department Number', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (63, N'NUM', N'^[0-9_]*$', N'Team Number', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (64, N'NAM', N'^[a-zA-Z0-9_]*$', N'Team Name', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (65, N'NAS', N'1', N'Non-Aligned Subteam', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (66, N'BA', N'^[a-zA-Z0-9 &]{1,10}$', N'Brand Abbreviation', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (67, N'TRM', N'^[\w \-\\/%<>&=\+]{1,150}$', N'Tax Romance', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (68, N'MDT', N'^[0-9]*$', N'Merch Default Tax Associatation', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (69, N'NCC', N'^[0-9]*$', N'National Class Code', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (70, N'HID', N'0|1', N'Hidden Item', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (71, N'NTS', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', N'Notes', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (72, N'ISI', N'^[a-zA-Z0-9_]*$', N'IRMA Store ID', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (73, N'SPT', N'^[a-zA-Z0-9_]*$', N'Store POS Type', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (74, N'FAX', N'^[a-zA-Z0-9_]*$', N'Fax', 5)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (75, N'DFC', N'1', N'Default Certification Agency', 7)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (76, N'AWR', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Animal Welfare Rating', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (77, N'BIO', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Biodynamic', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (78, N'CMT', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Cheese Milk Type', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (79, N'CR', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Cheese Raw', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (80, N'HER', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Healthy Eating Rating', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (82, N'ACH', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Air Chilled', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (83, N'DAG', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Dry Aged', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (84, N'FRR', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Free Range', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (85, N'GRF', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Grass Fed', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (86, N'MIH', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Made in House', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (87, N'MSC', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'MSC', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (88, N'PAS', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Pasture Raised', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (89, N'SFT', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Seafood Catch Type', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (90, N'SFF', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', N'Fresh or Frozen', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (91, N'RCN', N'', N'Recipe Name', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (92, N'ALR', N'', N'Allergens', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (93, N'ING', N'', N'Ingredients', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (94, N'SLM', N'', N'Selenium', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (95, N'PUF', N'', N'Polyunsaturated Fat', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (96, N'MUF', N'', N'Monounsaturated Fat', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (97, N'PTW', N'', N'Potassium Weight', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (98, N'PTP', N'', N'Potassium Percent', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (99, N'DFP', N'', N'Dietary Fiber Percent', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (100, N'SBF', N'', N'Soluble Fiber', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (101, N'ISF', N'', N'Insoluble Fiber', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (102, N'SAL', N'', N'Sugar Alcohol', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (103, N'OCH', N'', N'Other Carbohydrates', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (104, N'PPT', N'', N'Protein Percent', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (105, N'BCN', N'', N'Betacarotene', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (106, N'VTD', N'', N'Vitamin D', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (107, N'VTE', N'', N'Vitamin E', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (108, N'THM', N'', N'Thiamin', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (109, N'RFN', N'', N'Riboflavin', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (110, N'NAC', N'', N'Niacin', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (111, N'VB6', N'', N'Vitamin B6', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (112, N'FLT', N'', N'Folate', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (113, N'B12', N'', N'Vitamin B12', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (114, N'BTN', N'', N'Biotin', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (115, N'PAD', N'', N'Pantothenic Acid', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (116, N'PPH', N'', N'Phosphorous', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (117, N'IDN', N'', N'Iodine', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (118, N'MGM', N'', N'Magnesium', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (119, N'ZNC', N'', N'Zinc', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (120, N'CPR', N'', N'Copper', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (121, N'TSF', N'', N'Transfat', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (122, N'O6F', N'', N'Om6 Fatty', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (123, N'O3F', N'', N'Om3 Fatty', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (124, N'STR', N'', N'Starch', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (125, N'CHR', N'', N'Chloride', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (126, N'CHM', N'', N'Chromium', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (127, N'VTK', N'', N'Vitamin K', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (128, N'MGE', N'', N'Manganese', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (129, N'MBD', N'', N'Molybdenum', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (130, N'CTF', N'', N'Calories From Trans Fat', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (131, N'CSF', N'', N'Calories Saturated Fat', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (132, N'SPC', N'', N'Serving Per Container', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (133, N'SSD', N'', N'Serving Size Desc', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (134, N'SPP', N'', N'Servings Per Portion', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (135, N'SUT', N'', N'Serving Units', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (136, N'SWT', N'', N'Size Weight', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (137, N'TFW', N'', N'Transfat Weight', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (138, N'DS', N'^[a-zA-z ]+$', N'Delivery System', 1)
GO
INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (151, N'CFD', N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$', N'Customer Friendly Description', 1)
GO
SET IDENTITY_INSERT [dbo].[Trait] OFF
GO