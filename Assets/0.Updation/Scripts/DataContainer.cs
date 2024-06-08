using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
    public class SearchRoot
    {
        public List<User> users;
    }
[Serializable]
    public class User
    {
        public string id;
        public string first_name;
        public string last_name;
        public string email;
        public object wallet_address;
        public object ethereum_address;
        public bool followed_by_current_user;
    }

[Serializable]
public class CoinsCountRoot
{
    public bool success;
    public int points_count;
    public string last_rewarded_at;
}
[Serializable]
public class Daily_PointsRewardRoot
{
    public bool success;
    public string message;
}
[Serializable]
public class CoinsData
{
    public int available_tokens;
}
[Serializable]
public class RewardCoinsCountRoot
{
    public bool success;
    public CoinsData data;
}

[Serializable]
public class Data
{
    public int available_tokens;
}
[Serializable]
public class Root
{
    public bool success;
    public Data data;
}
[Serializable]
public class CoinsRoot
{
    public List<TokenRewardRule> token_reward_rules;
}
[Serializable]
public class TokenRewardRule
{
    public string rule_name;
    public int points;
    public bool reward_redeemed;
}
