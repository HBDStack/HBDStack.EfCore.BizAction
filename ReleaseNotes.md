# Release Notes

## TODO

 - Feature: Support AutoMapper `AutoMap` mapping approach (AutoMapper >= 8.0)

## 4.1.0.1
- Support model data validation for both `DataAnnotationsValidation` and `FluentValidation`. The validator need to be registed before use.

## 4.1.0
- Version 4.1.0 now requires you to install the NuGet package GenericServices.StatusGeneric in any of your assemblies that needs to interact with the GenericBizRunner's status.

## 4.0.0

- Support both EF Core >=2.1 and EF Core >=3.0 by supporting NetStandard2.0 and NetStandard2.1.
- Upgraded to AutoMapper version without static setup.
- Bug fix: GetAllErrors() should use Environment.NewLine.
- Style fix: Separator only has one E in it

## 3.0.0

- BREAKING CHANGE: The way you register GenericBizRunner at startup has changed - see [Upgrade Guide]() on how to do that. Fixes issue #6.
- BREAKING CHANGE: Changed the way in which AutoMapper mappings are set up. Please read the [Upgrade Guide]() on how this changes integration tests.
- Breaking change: removed AutoFac DI setup - now only provides Net Core setup (you can still use AutoFac for other DI usages).
- Improvement: You don't need need to call AutoMapper's code to find and set up DTO mappings on startup. That is now done inside the GenericBizRunner setup.
- New Feature: Added `IGenericStatus BeforeSaveChanges(DbContext)` to configuration. This allows you to inject code that is called just before SaveChanges/SaveChangesAsync is run. This allows you to add some validation, logging etc.
- Minor improvement: Previously the Sql error handler was only used if validation was turned on. Now, if the SaveChangesExceptionHandler property is not null, then that method is called, i.e. it is not longer dependant on the state of the ...ValidateOnSave flag in the config. 

## 2.0.1

- Bug Fix: Fixed a error in StatusGenericHandler where `CombineErrors` didn't copy over the `Message` properly

## 2.0.0

- Breaking Change: The SaveChangesWithValidation/Async interface has changed to accommodate the new SaveChangesExceptionHandler.
- New Feature: Added SqlErrorHandler to configuration and called in SaveChangesWithValidation/Async. Allows you to intercept an exception in SaveChanges and do things like capture SQL errors and turn them into user-friendly error messages.
- New Feature: A version of the GenericBizRunner dependency injection registration that works with NET Core DI provider.
- Minor change: added GetAllErrors method to IBizActionStatus to get all errors in one go. Mainly useful for unit testing.

## 1.1.1 (unlisted as replaced by 2.0.0)

- New Feature: Added SqlErrorHandler to configuration and called in SaveChangesWithValidation/Async.
Allows you to intercept `DbUpdateException` and turn SQL errors into user-friendly error messages.
- New Feature: A version of the GenericBizRunner dependency injection registration that works with NET Core DI provider 
- Minor change: added GetAllErrors method to IBizActionStatus to get all errors in one go.
Mainly useful for unit testing. 

## 1.1.0

- Package: Updated to NET Core 2.1

## 1.0.0 

- First release