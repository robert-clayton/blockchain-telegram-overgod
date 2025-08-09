// SpacetimeDB module sketch for OvergodIdle
// Replace placeholder macros/types with actual spacetimedb imports in your environment

// use spacetimedb::prelude::*;

#[allow(dead_code)]
pub struct Timestamp(i64);

// #[table]
#[allow(dead_code)]
pub struct Player {
    // #[primary_key]
    pub user_id: i64,
    pub username: String,
    pub best_score: i64,
    pub updated_at: Timestamp,
}

// #[table]
#[allow(dead_code)]
pub struct SeasonScore {
    // #[primary_key]
    pub season_id: i32,
    // #[index]
    pub user_id: i64,
    pub score: i64,
    pub updated_at: Timestamp,
}

// #[spacetimedb(init)]
#[allow(dead_code)]
pub fn init() {}

// #[spacetimedb]
#[allow(dead_code)]
pub fn post_score(_season_id: i32, _user_id: i64, _username: String, _score: i64) {
    // TODO: implement transactional upsert for SeasonScore and Player.best_score
}

// #[spacetimedb(view)]
#[allow(dead_code)]
pub fn top_n(_season_id: i32, _n: i32) -> Vec<(String, i64)> {
    // TODO: implement query return (username, score) for top N
    Vec::new()
}


