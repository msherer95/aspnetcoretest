TODO:

Refresh tokens:
1. Issue, save in db (as a hash) with expiration, and a device ID
2. Detect expirey of access token in client and automatically try to get a new refresh token
3. Check if refresh token in db matches that of device ID and hasn't expired yet
4. If good, issue new refresh & access tokens
5. If not good, throw a 401 again, saying that refresh expired. Client will handle a new login prompt

Refresh claims via middleware:
1. Store all logged in users
    - Need this to manage other user sessions
2. Middleware for: notifying of claims change, revoking access_token, ending session (sign out)-- store in singleton service
3. When user issues a new request, check if they require changes from this service and apply them.
