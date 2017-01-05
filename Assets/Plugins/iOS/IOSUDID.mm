//
//  TinyiOSLib.m
//  TinyiOSLib
//
//  Created by Nevermore_Tiny on 6/3/16.
//  Copyright © 2016 Nevermore_Tiny. All rights reserved.
//
#import <Security/Security.h>
#include <sys/socket.h>
#include <sys/sysctl.h>
#include <net/if.h>
#include <net/if_dl.h>
#include <string>
#include <ifaddrs.h>
#include <arpa/inet.h>
#include <net/if.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <UIKit/UIAlert.h>


static const char kKeychainUDIDItemIdentifier[]  = "DDW_UUID";

const char* getUDIDFromKeyChain()
{
    NSMutableDictionary *dictForQuery = [[NSMutableDictionary alloc] init];
    [dictForQuery setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
    
    // set Attr Identity for query
    NSData *keychainItemID = [NSData dataWithBytes:kKeychainUDIDItemIdentifier length:strlen(kKeychainUDIDItemIdentifier)];
    [dictForQuery setObject:keychainItemID forKey:(id)kSecAttrGeneric];
    
    [dictForQuery setValue:(id)kCFBooleanTrue forKey:(id)kSecMatchCaseInsensitive];
    [dictForQuery setValue:(id)kSecMatchLimitOne forKey:(id)kSecMatchLimit];
    [dictForQuery setValue:(id)kCFBooleanTrue forKey:(id)kSecReturnData];
    
    OSStatus queryErr   = noErr;
    NSData   *udidValue = nil;
    NSString *udid      = nil;
    
    CFDataRef resultRef = nil;
    queryErr = SecItemCopyMatching((__bridge CFDictionaryRef)dictForQuery,
                                          (CFTypeRef *)&resultRef);
    udidValue = (__bridge_transfer NSData*)resultRef;
    //queryErr = SecItemCopyMatching((CFDictionaryRef)dictForQuery, (CFTypeRef*)&udidValue);
    
    if (queryErr == errSecItemNotFound) {
        NSLog(@"KeyChain Item: %@ not found!!!", [NSString stringWithUTF8String:kKeychainUDIDItemIdentifier]);
    }
    else if (queryErr != errSecSuccess) {
        NSLog(@"KeyChain Item query Error!!! Error code:%d", (int)queryErr);
    }
    if (queryErr == errSecSuccess) {
        NSLog(@"KeyChain Item: %@", udidValue);
        
        if (udidValue) {
            udid = [NSString stringWithUTF8String:(char*)udidValue.bytes];
        }
    }
    
    //[dictForQuery release];
    return [udid UTF8String];
}

bool updateUDIDInKeyChain(const char* _newUDID)
{
    NSString *newUDID;
    newUDID = [[NSString alloc] initWithUTF8String:_newUDID];
    
    NSMutableDictionary *dictForQuery = [[NSMutableDictionary alloc] init];
    
    [dictForQuery setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
    
    NSData *keychainItemID = [NSData dataWithBytes:kKeychainUDIDItemIdentifier length:strlen(kKeychainUDIDItemIdentifier)];
    [dictForQuery setValue:keychainItemID forKey:(id)kSecAttrGeneric];
    [dictForQuery setValue:(id)kCFBooleanTrue forKey:(id)kSecMatchCaseInsensitive];
    [dictForQuery setValue:(id)kSecMatchLimitOne forKey:(id)kSecMatchLimit];
    [dictForQuery setValue:(id)kCFBooleanTrue forKey:(id)kSecReturnAttributes];
    
    NSDictionary *queryResult = nil;
    CFDataRef resultRef = nil;
    SecItemCopyMatching((__bridge CFDictionaryRef)dictForQuery,
                                   (CFTypeRef *)&resultRef);
    
    //SecItemCopyMatching((CFDictionaryRef)dictForQuery, (CFTypeRef*)&queryResult);
    if (queryResult) {
        NSMutableDictionary *dictForUpdate = [[NSMutableDictionary alloc] init];
        [dictForUpdate setValue:keychainItemID forKey:(id)kSecAttrGeneric];
        
        const char *udidStr = [newUDID UTF8String];
        NSData *keyChainItemValue = [NSData dataWithBytes:udidStr length:strlen(udidStr)];
        [dictForUpdate setValue:keyChainItemValue forKey:(id)kSecValueData];
        
        OSStatus updateErr = noErr;
        
        // First we need the attributes from the Keychain.
        NSMutableDictionary *updateItem = [NSMutableDictionary dictionaryWithDictionary:queryResult];
        
        // Second we need to add the appropriate search key/values.
        // set kSecClass is Very important
        [updateItem setObject:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        
        updateErr = SecItemUpdate((CFDictionaryRef)updateItem, (CFDictionaryRef)dictForUpdate);
        if (updateErr != errSecSuccess) {
            NSLog(@"Update KeyChain Item Error!!! Error Code:%d", (int)updateErr);
            
            //[dictForQuery release];
            //[dictForUpdate release];
            return NO;
        }
        else {
            NSLog(@"Update KeyChain Item Success!!!");
            //[dictForQuery release];
            //[dictForUpdate release];
            return YES;
        }
    }
    
    //[dictForQuery release];
    return NO;
}

bool settUDIDToKeyChain(const char* _udid)
{
    NSString *udid;
    udid = [[NSString alloc] initWithUTF8String:_udid];
    const char *udidStr = [udid UTF8String];
    
    if (getUDIDFromKeyChain())
    {
        updateUDIDInKeyChain(udidStr);
        return YES;
    }
    else {
        NSMutableDictionary *dicForQuery = [[NSMutableDictionary alloc] init];
        [dicForQuery setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        [dicForQuery setValue:(id)kCFBooleanTrue forKey:(id)kSecReturnData];
        OSStatus queryErr   = noErr;
        NSData   *udidValue = nil;
        NSString *udid      = nil;
        
        CFDataRef resultRef = nil;
        queryErr = SecItemCopyMatching((__bridge CFDictionaryRef)dicForQuery,
                                       (CFTypeRef *)&resultRef);
        udidValue = (__bridge_transfer NSData*)resultRef;
        //queryErr = SecItemCopyMatching((CFDictionaryRef)dicForQuery, (CFTypeRef*)&udidValue);
        if (queryErr == errSecSuccess)
        {
            if (udidValue) {
                udid = [NSString stringWithUTF8String:(char*)udidValue.bytes];
                udidStr=[udid UTF8String];
            }
            NSMutableDictionary *dictToDelete = [[NSMutableDictionary alloc] init];
            [dictToDelete setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
            OSStatus deleteErr = noErr;
            deleteErr = SecItemDelete((CFDictionaryRef)dictToDelete);
            if (deleteErr != errSecSuccess) {
                NSLog(@"delete UUID from KeyChain Error!!! Error code:%d", (int)deleteErr);
            }
            else {
                NSLog(@"delete success!!!");
            }
            //[dictToDelete release];
        }
        //[dicForQuery release];
        
        NSMutableDictionary *dictForAdd = [[NSMutableDictionary alloc] init];
        [dictForAdd setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
        NSData *keyChainItemID = [NSData dataWithBytes:kKeychainUDIDItemIdentifier length:strlen(kKeychainUDIDItemIdentifier)];
        [dictForAdd setValue: keyChainItemID forKey:(id)kSecAttrGeneric];
        
        
        
        NSData *keyChainItemValue = [NSData dataWithBytes:udidStr length:strlen(udidStr)];
        [dictForAdd setValue:keyChainItemValue forKey:(id)kSecValueData];
        
        OSStatus writeErr = noErr;
        writeErr = SecItemAdd((CFDictionaryRef)dictForAdd, NULL);
        if (writeErr != errSecSuccess) {
            NSLog(@"Add KeyChain Item Error!!! Error Code:%d", (int)writeErr);
            
            //[dictForAdd release];
            return NO;
        }
        else {
            NSLog(@"Add KeyChain Item Success!!!");
            //[dictForAdd release];
            return YES;
        }
        //[dictForAdd release];
    }
    return NO;
}

bool removeUDIDFromKeyChain()
{
    NSMutableDictionary *dictToDelete = [[NSMutableDictionary alloc] init];
    
    [dictToDelete setValue:(id)kSecClassGenericPassword forKey:(id)kSecClass];
    
    NSData *keyChainItemID = [NSData dataWithBytes:kKeychainUDIDItemIdentifier length:strlen(kKeychainUDIDItemIdentifier)];
    [dictToDelete setValue:keyChainItemID forKey:(id)kSecAttrGeneric];
    
    OSStatus deleteErr = noErr;
    deleteErr = SecItemDelete((CFDictionaryRef)dictToDelete);
    if (deleteErr != errSecSuccess) {
        NSLog(@"delete UUID from KeyChain Error!!! Error code:%d", (int)deleteErr);
        //[dictToDelete release];
        return NO;
    }
    else {
        NSLog(@"delete success!!!");
    }
    
    //[dictToDelete release];
    return true;
}


#pragma mark -
#pragma mark Helper Method for Get Mac Address

const char* getMacAddress()
{
    int                 mgmtInfoBase[6];
    char                *msgBuffer = NULL;
    size_t              length;
    unsigned char       macAddress[6];
    struct if_msghdr    *interfaceMsgStruct;
    struct sockaddr_dl  *socketStruct;
    NSString            *errorFlag = nil;
    
    // Setup the management Information Base (mib)
    mgmtInfoBase[0] = CTL_NET;        // Request network subsystem
    mgmtInfoBase[1] = AF_ROUTE;       // Routing table info
    mgmtInfoBase[2] = 0;
    mgmtInfoBase[3] = AF_LINK;        // Request link layer information
    mgmtInfoBase[4] = NET_RT_IFLIST;  // Request all configured interfaces
    
    // With all configured interfaces requested, get handle index
    if ((mgmtInfoBase[5] = if_nametoindex("en0")) == 0)
        errorFlag = @"if_nametoindex failure";
    else
    {
        // Get the size of the data available (store in len)
        if (sysctl(mgmtInfoBase, 6, NULL, &length, NULL, 0) < 0)
            errorFlag = @"sysctl mgmtInfoBase failure";
        else
        {
            // Alloc memory based on above call
            if ((msgBuffer = (char*)malloc(length)) == NULL)
                errorFlag = @"buffer allocation failure";
            else
            {
                // Get system information, store in buffer
                if (sysctl(mgmtInfoBase, 6, msgBuffer, &length, NULL, 0) < 0)
                    errorFlag = @"sysctl msgBuffer failure";
            }
        }
    }
    
    // Befor going any further...
    if (errorFlag != NULL)
    {
        NSLog(@"Error: %@", errorFlag);
        if (msgBuffer) {
            free(msgBuffer);
        }
        
        return [errorFlag UTF8String];
    }
    
    // Map msgbuffer to interface message structure
    interfaceMsgStruct = (struct if_msghdr *) msgBuffer;
    
    // Map to link-level socket structure
    socketStruct = (struct sockaddr_dl *) (interfaceMsgStruct + 1);
    
    // Copy link layer address data in socket structure to an array
    memcpy(&macAddress, socketStruct->sdl_data + socketStruct->sdl_nlen, 6);
    
    // Read from char array into a string object, into traditional Mac address format
    NSString *macAddressString = [NSString stringWithFormat:@"%02X:%02X:%02X:%02X:%02X:%02X",
                                  macAddress[0], macAddress[1], macAddress[2],
                                  macAddress[3], macAddress[4], macAddress[5]];
    NSLog(@"Mac Address: %@", macAddressString);
    
    free(msgBuffer);
    
    return [macAddressString UTF8String];
}


/*
 * iOS 6.0
 * use wifi's mac address
 */
const char* _UDID_iOS6()
{
    const char *udid =  getUDIDFromKeyChain();
    if (!udid)
    {
        udid = getMacAddress();
        settUDIDToKeyChain(udid);
    }
    return udid;
}

/*
 * iOS 7.0
 * Starting from iOS 7, the system always returns the value 02:00:00:00:00:00
 * when you ask for the MAC address on any device.
 * use identifierForVendor + keyChain
 * make sure UDID consistency atfer app delete and reinstall
 */
const char* _UDID_iOS7()
{
    const char *udid =  getUDIDFromKeyChain();
    if (!udid)
    {
        udid = [[[UIDevice currentDevice].identifierForVendor UUIDString] UTF8String];
        settUDIDToKeyChain(udid);
    }
    return getUDIDFromKeyChain();
}

extern "C"{
    char* _MakeStringCopy(const char* inString){
        if(inString == NULL){
            return NULL;
        }
        char* output = (char*)malloc(strlen(inString)+1);
        strcpy(output, inString);
        return output;
    }
    
    const char* _GetUDID()
    {
        NSString *sysVersion = [UIDevice currentDevice].systemVersion;
        CGFloat version = [sysVersion floatValue];
        if (version >= 7.0)
        {
            return _MakeStringCopy(_UDID_iOS7());
        }
        else
        {
            return _MakeStringCopy(_UDID_iOS6());
        }
        return NULL;
    }
    
}





